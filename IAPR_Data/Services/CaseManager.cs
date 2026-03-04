using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using IAPR_Data.Classes;

namespace IAPR_Data.Services
{
    /// <summary>
    /// Manages compliance cases opened automatically by the <see cref="ComplianceEngine"/>.
    ///
    /// Responsibilities:
    /// - Open a case for NonCompliant / PendingReview outcomes.
    /// - Calculate SLA deadline from outcome priority.
    /// - Escalate overdue open/in-progress cases (background poll, every 5 min).
    /// - Provide a Resolve operation for agents.
    ///
    /// Call <see cref="Start"/> once from Application_Start.
    /// </summary>
    public sealed class CaseManager : IDisposable
    {
        private static readonly Lazy<CaseManager> _instance =
            new Lazy<CaseManager>(() => new CaseManager());

        public static CaseManager Instance => _instance.Value;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Thread _escalationThread;
        private volatile bool _isRunning;

        /// <summary>How often the background SLA escalation sweep runs.</summary>
        private const int EscalationPollMs = 5 * 60 * 1000; // 5 minutes

        // SLA windows by priority
        private static readonly Dictionary<CasePriority, TimeSpan> SlaWindows =
            new Dictionary<CasePriority, TimeSpan>
            {
                { CasePriority.Critical, TimeSpan.FromHours(4)   },
                { CasePriority.High,     TimeSpan.FromHours(24)  },
                { CasePriority.Medium,   TimeSpan.FromHours(72)  },
                { CasePriority.Low,      TimeSpan.FromDays(7)    },
            };

        private CaseManager()
        {
            _escalationThread = new Thread(EscalationLoop)
            {
                IsBackground = true,
                Name = "CaseManager-Escalation"
            };
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            _escalationThread.Start();
            System.Diagnostics.Trace.TraceInformation("[CaseManager] Started escalation watcher.");
        }

        public void Stop() => _cts.Cancel();

        public void Dispose()
        {
            Stop();
            _cts.Dispose();
        }

        // ------------------------------------------------------------------
        // Open a new case from a ComplianceState outcome
        // ------------------------------------------------------------------

        /// <summary>
        /// Opens a compliance case for a NonCompliant or PendingReview outcome.
        /// Must be called inside an existing EF transaction — this method adds entities to <paramref name="db"/>
        /// but does NOT call SaveChanges (the caller's transaction handles that).
        /// </summary>
        public void OpenCase(ApplicationDbContext db, ComplianceState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            // Determine priority from outcome
            var priority = state.Outcome == ComplianceOutcome.NonCompliant.ToString()
                ? DerivePriority(state.Reason)
                : CasePriority.Medium; // PendingReview = Medium by default

            var now    = DateTime.UtcNow;
            var dueAt  = now.Add(SlaWindows[priority]);
            var caseNumber = GenerateCaseNumber(db, now.Year);

            var newCase = new Case
            {
                CaseNumber             = caseNumber,
                Title                  = $"Compliance: {state.Outcome} — {TruncateReason(state.Reason)}",
                Description            = state.Reason,
                Status                 = CaseStatus.Open.ToString(),
                Priority               = priority.ToString(),
                SourceComplianceStateId = state.Id,
                TenantId               = state.TenantId,
                CorrelationId          = state.CorrelationId,
                DueAt                  = dueAt
            };

            db.Cases.Add(newCase);

            // Initial system note
            db.CaseNotes.Add(new CaseNote
            {
                Case              = newCase,
                AuthorName        = "System",
                NoteText          = $"Case opened automatically by ComplianceEngine. Outcome: {state.Outcome}. " +
                                    $"SLA deadline: {dueAt:yyyy-MM-dd HH:mm} UTC. Source event: {state.SourceEventId}.",
                IsSystemGenerated = true
            });

            // Audit log
            AuditLogger.Log(db,
                entityName:    "Case",
                entityId:      caseNumber,
                action:        "Created",
                newValues:     new { newCase.CaseNumber, newCase.Status, newCase.Priority, newCase.DueAt },
                actorName:     "ComplianceEngine",
                tenantId:      state.TenantId,
                correlationId: state.CorrelationId,
                notes:         $"Auto-opened from ComplianceState source event {state.SourceEventId}");
        }

        // ------------------------------------------------------------------
        // Resolve a case (agent action)
        // ------------------------------------------------------------------

        /// <summary>
        /// Marks a case as Resolved. Writes an agent note and an audit log entry.
        /// </summary>
        public bool ResolveCase(int caseId, string resolution, string actorUserId = null, string actorName = null)
        {
            try
            {
                using (var db = ApplicationDbContext.Create())
                using (var tx = db.Database.BeginTransaction())
                {
                    var cas = db.Cases.Find(caseId);
                    if (cas == null) return false;

                    var now = DateTime.UtcNow;
                    cas.Status     = CaseStatus.Resolved.ToString();
                    cas.ResolvedAt = now;

                    db.CaseNotes.Add(new CaseNote
                    {
                        CaseId            = caseId,
                        AuthorUserId      = actorUserId,
                        AuthorName        = actorName ?? "Agent",
                        NoteText          = $"Case resolved. Resolution: {resolution}",
                        IsSystemGenerated = false
                    });

                    AuditLogger.Log(db,
                        entityName:   "Case",
                        entityId:     caseId.ToString(),
                        action:       "Resolved",
                        oldValues:    new { Status = "Open" },
                        newValues:    new { Status = "Resolved", ResolvedAt = now },
                        actorUserId:  actorUserId,
                        actorName:    actorName ?? "Agent",
                        tenantId:     cas.TenantId,
                        correlationId: cas.CorrelationId,
                        notes:        resolution);

                    db.SaveChanges();
                    tx.Commit();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError($"[CaseManager] ResolveCase({caseId}) failed: {ex.Message}");
                return false;
            }
        }

        // ------------------------------------------------------------------
        // Background SLA escalation sweep
        // ------------------------------------------------------------------

        private void EscalationLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    EscalateOverdueCases();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("[CaseManager] Escalation sweep error: " + ex.Message);
                }

                Thread.Sleep(EscalationPollMs);
            }
        }

        private void EscalateOverdueCases()
        {
            var openStatuses = new[] { CaseStatus.Open.ToString(), CaseStatus.InProgress.ToString() };
            var now = DateTime.UtcNow;

            using (var db = ApplicationDbContext.Create())
            using (var tx = db.Database.BeginTransaction())
            {
                var overdue = db.Cases
                    .Where(c => openStatuses.Contains(c.Status) && c.DueAt < now)
                    .ToList();

                if (!overdue.Any())
                {
                    tx.Rollback();
                    return;
                }

                foreach (var cas in overdue)
                {
                    cas.Status      = CaseStatus.Escalated.ToString();
                    cas.EscalatedAt = now;

                    db.CaseNotes.Add(new CaseNote
                    {
                        CaseId            = cas.Id,
                        AuthorName        = "System",
                        NoteText          = $"⚠ SLA breach: case was due {cas.DueAt:yyyy-MM-dd HH:mm} UTC. Auto-escalated at {now:yyyy-MM-dd HH:mm} UTC.",
                        IsSystemGenerated = true
                    });

                    AuditLogger.Log(db,
                        entityName:    "Case",
                        entityId:      cas.Id.ToString(),
                        action:        "Escalated",
                        oldValues:     new { cas.Status, cas.DueAt },
                        newValues:     new { Status = "Escalated", EscalatedAt = now },
                        actorName:     "CaseManager-SLA",
                        tenantId:      cas.TenantId,
                        correlationId: cas.CorrelationId,
                        notes:         "SLA breach auto-escalation");
                }

                db.SaveChanges();
                tx.Commit();

                System.Diagnostics.Trace.TraceInformation(
                    $"[CaseManager] Escalated {overdue.Count} overdue case(s).");
            }
        }

        // ------------------------------------------------------------------
        // Helpers
        // ------------------------------------------------------------------

        private static CasePriority DerivePriority(string reason)
        {
            if (string.IsNullOrEmpty(reason)) return CasePriority.Medium;
            var r = reason.ToLowerInvariant();
            if (r.Contains("aml.flagged") || r.Contains("kyc.failed") || r.Contains("fraud"))
                return CasePriority.Critical;
            if (r.Contains("cancelled") || r.Contains("lapsed") || r.Contains("suspended"))
                return CasePriority.High;
            if (r.Contains("overdue") || r.Contains("missed"))
                return CasePriority.Medium;
            return CasePriority.Low;
        }

        private static string GenerateCaseNumber(ApplicationDbContext db, int year)
        {
            // Count existing cases for this year to produce a sequential suffix
            var prefix  = $"CASE-{year}-";
            var count   = db.Cases.Count(c => c.CaseNumber.StartsWith(prefix));
            return $"{prefix}{(count + 1):D5}";
        }

        private static string TruncateReason(string reason, int maxLen = 80)
        {
            if (string.IsNullOrEmpty(reason)) return "See case description";
            return reason.Length <= maxLen ? reason : reason.Substring(0, maxLen - 3) + "...";
        }
    }
}
