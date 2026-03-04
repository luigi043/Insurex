using System;
using System.Linq;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;
using Newtonsoft.Json;

namespace IAPR_Data.Services
{
    /// <summary>
    /// The Compliance Engine subscribes to <see cref="WebhookEventQueue"/> and deterministically
    /// evaluates incoming insurer events against the platform's compliance rule set.
    ///
    /// Design principles:
    /// - Deterministic: same input always produces same <see cref="ComplianceOutcome"/>.
    /// - Atomic: each evaluation writes <see cref="ComplianceState"/> + <see cref="OutboxMessage"/>
    ///   + <see cref="AuditLogEntry"/> (and a <see cref="Case"/> for non-compliant outcomes)
    ///   in a single DB transaction (Outbox pattern + Audit trail).
    /// - Idempotent: duplicate EventIds are detected and skipped.
    /// - Pluggable: rule evaluation is delegated to <see cref="EvaluateRules"/> which can be
    ///   extended with new event types without touching the orchestration logic.
    /// </summary>
    public sealed class ComplianceEngine
    {
        private static readonly Lazy<ComplianceEngine> _instance =
            new Lazy<ComplianceEngine>(() => new ComplianceEngine());

        /// <summary>Singleton access point.</summary>
        public static ComplianceEngine Instance => _instance.Value;

        private bool _isRegistered;

        private ComplianceEngine() { }

        /// <summary>
        /// Registers this engine as the message processor on the singleton
        /// <see cref="WebhookEventQueue"/> and starts the queue worker.
        /// Call once from Application_Start.
        /// </summary>
        public void Start()
        {
            if (_isRegistered) return;

            WebhookEventQueue.Instance.OnMessage = ProcessEvent;
            WebhookEventQueue.Instance.Start();
            _isRegistered = true;

            System.Diagnostics.Trace.TraceInformation("[ComplianceEngine] Started and registered with WebhookEventQueue.");
        }

        /// <summary>
        /// Processes a single webhook event message dispatched from the queue.
        /// </summary>
        private void ProcessEvent(WebhookEventMessage message)
        {
            if (message == null) return;

            System.Diagnostics.Trace.TraceInformation(
                $"[ComplianceEngine] Processing event {message.EventId} | type={message.EventType} | source={message.Source}");

            using (var db = ApplicationDbContext.Create())
            {
                // --- 1. Idempotency guard: skip if already evaluated ---
                bool alreadyEvaluated = db.ComplianceStates
                    .Any(cs => cs.SourceEventId == message.EventId);

                if (alreadyEvaluated)
                {
                    System.Diagnostics.Trace.TraceInformation(
                        $"[ComplianceEngine] Event {message.EventId} already evaluated — skipping.");
                    return;
                }

                // --- 2. Run the deterministic rule evaluation ---
                var (outcome, reason) = EvaluateRules(message);

                // --- 3. Build the compliance state record ---
                var state = new ComplianceState
                {
                    SourceEventId  = message.EventId,
                    Outcome        = outcome.ToString(),
                    Reason         = reason,
                    TenantId       = message.TenantId,
                    CorrelationId  = message.EventId // re-use EventId as correlation chain root
                };

                // Attempt to extract PolicyId / AssetId from payload
                TryEnrichFromPayload(message.Payload, state);

                // --- 4. Build the outbox message (published downstream after commit) ---
                var outboxMsg = new OutboxMessage
                {
                    MessageType   = "ComplianceOutcomeEmitted",
                    Payload       = JsonConvert.SerializeObject(new
                    {
                        state.SourceEventId,
                        state.Outcome,
                        state.Reason,
                        state.PolicyId,
                        state.AssetId,
                        state.TenantId,
                        state.CorrelationId,
                        state.EvaluatedAt
                    }),
                    CorrelationId = state.CorrelationId,
                    TenantId      = message.TenantId
                };

                // --- 5. Atomic write: ComplianceState + OutboxMessage in one transaction ---
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.ComplianceStates.Add(state);
                        db.OutboxMessages.Add(outboxMsg);

                        // --- Phase 4: Audit log entry for this evaluation ---
                        AuditLogger.Log(db,
                            entityName:    "ComplianceState",
                            entityId:      message.EventId,
                            action:        "Evaluated",
                            newValues:     new { state.Outcome, state.Reason, state.EvaluatedAt },
                            actorName:     "ComplianceEngine",
                            tenantId:      message.TenantId,
                            correlationId: state.CorrelationId,
                            notes:         $"Event type: {message.EventType} | Source: {message.Source}");

                        // --- Phase 4: Open a compliance case for adverse outcomes ---
                        if (outcome == ComplianceOutcome.NonCompliant ||
                            outcome == ComplianceOutcome.PendingReview)
                        {
                            CaseManager.Instance.OpenCase(db, state);
                        }

                        // Mark the source webhook event as Processed
                        var webhookEvent = db.WebhookEvents
                            .FirstOrDefault(w => w.EventId == message.EventId);
                        if (webhookEvent != null)
                        {
                            webhookEvent.Status      = "Processed";
                            webhookEvent.ProcessedAt = DateTime.UtcNow;
                        }

                        db.SaveChanges();
                        tx.Commit();

                        System.Diagnostics.Trace.TraceInformation(
                            $"[ComplianceEngine] Event {message.EventId} → outcome={outcome} | reason={reason}");
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        System.Diagnostics.Trace.TraceError(
                            $"[ComplianceEngine] DB write failed for event {message.EventId}: {ex.Message}");
                        throw; // re-throw so the queue logs it and marks the event Failed
                    }
                }
            }
        }

        // ------------------------------------------------------------------
        // Rule Evaluation
        // ------------------------------------------------------------------

        /// <summary>
        /// Deterministic rule engine: maps event type + payload to a compliance outcome.
        /// Add new case branches here as more event types are onboarded.
        /// </summary>
        private static (ComplianceOutcome outcome, string reason) EvaluateRules(WebhookEventMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.EventType))
                return (ComplianceOutcome.PendingReview, "Event type is missing; unable to classify.");

            var evtType = message.EventType.ToLowerInvariant().Trim();

            // --- POLICY EVENTS ---
            if (evtType == "policy.created" || evtType == "policy.renewed")
                return (ComplianceOutcome.Compliant, $"Policy lifecycle event '{message.EventType}' accepted. Coverage verified.");

            if (evtType == "policy.cancelled" || evtType == "policy.lapsed")
                return (ComplianceOutcome.NonCompliant, $"Policy is no longer active ('{message.EventType}'). Immediate remediation required.");

            if (evtType == "policy.suspended")
                return (ComplianceOutcome.NonCompliant, $"Policy suspended ('{message.EventType}'). Asset protection suspended.");

            // --- CLAIM EVENTS ---
            if (evtType == "claim.submitted")
                return (ComplianceOutcome.Compliant, "Claim submission received. Active monitoring initiated.");

            if (evtType == "claim.approved")
                return (ComplianceOutcome.Compliant, "Claim approved by insurer. Settlement process compliant.");

            if (evtType == "claim.rejected")
                return (ComplianceOutcome.NonCompliant, "Claim rejected by insurer. Review coverage gap with client.");

            if (evtType == "claim.withdrawn")
                return (ComplianceOutcome.PendingReview, "Claim withdrawn. Manual review required to confirm no outstanding liability.");

            // --- PAYMENT / PREMIUM EVENTS ---
            if (evtType == "premium.paid")
                return (ComplianceOutcome.Compliant, "Premium payment confirmed. Policy in good standing.");

            if (evtType == "premium.overdue" || evtType == "premium.missed")
                return (ComplianceOutcome.NonCompliant, $"Premium payment overdue ('{message.EventType}'). Policy at risk of lapse.");

            // --- ENDORSEMENT EVENTS ---
            if (evtType == "policy.endorsed")
                return (ComplianceOutcome.Compliant, "Policy endorsement processed. Updated terms accepted.");

            if (evtType == "policy.endorsement.rejected")
                return (ComplianceOutcome.PendingReview, "Endorsement rejected by insurer. Review required before proceeding.");

            // --- AUDIT / KYC EVENTS ---
            if (evtType == "kyc.completed" || evtType == "aml.cleared")
                return (ComplianceOutcome.Compliant, $"Due diligence check '{message.EventType}' completed successfully.");

            if (evtType == "kyc.failed" || evtType == "aml.flagged")
                return (ComplianceOutcome.NonCompliant, $"Compliance flag raised: '{message.EventType}'. Escalate to compliance officer immediately.");

            // --- FALLTHROUGH ---
            return (ComplianceOutcome.Ignored, $"Unrecognised event type '{message.EventType}'. No compliance action taken.");
        }

        /// <summary>
        /// Attempts to extract PolicyId and AssetId from the JSON payload to enrich the state record.
        /// Failures are silently swallowed — enrichment is best-effort.
        /// </summary>
        private static void TryEnrichFromPayload(string payload, ComplianceState state)
        {
            if (string.IsNullOrWhiteSpace(payload)) return;
            try
            {
                dynamic parsed = JsonConvert.DeserializeObject(payload);
                if (parsed?.policyId != null)
                    state.PolicyId = (int?)parsed.policyId;
                if (parsed?.assetId != null)
                    state.AssetId = (int?)parsed.assetId;
            }
            catch { /* payload enrichment is best-effort */ }
        }
    }
}
