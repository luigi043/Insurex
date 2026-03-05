using System;
using System.Linq;
using IAPR_Data.Classes;
using IAPR_Data.Classes.Webhook;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IAPR_Data.Services
{
    /// <summary>
    /// The Compliance Engine evaluatues incoming insurer events against the platform's compliance rule set.
    /// Modernized for .NET 8 with Dependency Injection.
    /// </summary>
    public sealed class ComplianceEngine
    {
        private readonly ILogger<ComplianceEngine> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

        public ComplianceEngine(ILogger<ComplianceEngine> logger, IDbContextFactory<ApplicationDbContext> dbFactory)
        {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        /// <summary>
        /// Processes a single webhook event message.
        /// </summary>
        public async Task ProcessEventAsync(WebhookEventMessage message)
        {
            if (message == null) return;

            _logger.LogInformation("Processing event {EventId} | type={EventType} | source={Source}", 
                message.EventId, message.EventType, message.Source);

            using (var db = await _dbFactory.CreateDbContextAsync())
            {
                // --- 1. Idempotency guard ---
                bool alreadyEvaluated = await db.ComplianceStates
                    .AnyAsync(cs => cs.SourceEventId == message.EventId);

                if (alreadyEvaluated)
                {
                    _logger.LogInformation("Event {EventId} already evaluated — skipping.", message.EventId);
                    return;
                }

                // --- 2. Run rule evaluation ---
                var (outcome, reason) = EvaluateRules(message);

                // --- 3. Build state ---
                var state = new ComplianceState
               {
                    SourceEventId  = message.EventId,
                    Outcome        = outcome.ToString(),
                    Reason         = reason,
                    TenantId       = message.TenantId,
                    CorrelationId  = message.EventId,
                    EvaluatedAt    = DateTime.UtcNow
                };

                TryEnrichFromPayload(message.Payload, state);

                // --- 4. Build outbox message ---
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

                // --- 5. Atomic write ---
                using (var tx = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        db.ComplianceStates.Add(state);
                        db.OutboxMessages.Add(outboxMsg);

                        // Audit log
                        AuditLogger.Log(db,
                            entityName:    "ComplianceState",
                            entityId:      message.EventId,
                            action:        "Evaluated",
                            newValues:     new { state.Outcome, state.Reason, state.EvaluatedAt },
                            actorName:     "ComplianceEngine",
                            tenantId:      message.TenantId,
                            correlationId: state.CorrelationId,
                            notes:         $"Event type: {message.EventType} | Source: {message.Source}");

                        // Open case for non-compliance
                        if (outcome == ComplianceOutcome.NonCompliant ||
                            outcome == ComplianceOutcome.PendingReview)
                        {
                            // In .NET 8, CaseManager should also be a DI service
                            // For now, we'll assume it's handled or we'll refactor it next
                            _logger.LogWarning("Adverse outcome for {EventId} - Case required.", message.EventId);
                        }

                        // Mark webhook processed
                        var webhookEvent = await db.WebhookEvents
                            .FirstOrDefaultAsync(w => w.EventId == message.EventId);
                        if (webhookEvent != null)
                        {
                            webhookEvent.Status      = "Processed";
                            webhookEvent.ProcessedAt = DateTime.UtcNow;
                        }

                        await db.SaveChangesAsync();
                        await tx.CommitAsync();

                        _logger.LogInformation("Event {EventId} → outcome={Outcome} | reason={Reason}", 
                            message.EventId, outcome, reason);
                    }
                    catch (Exception ex)
                    {
                        await tx.RollbackAsync();
                        _logger.LogError(ex, "DB write failed for event {EventId}", message.EventId);
                        throw;
                    }
                }
            }
        }

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

        private static void TryEnrichFromPayload(string? payload, ComplianceState state)
        {
            if (string.IsNullOrWhiteSpace(payload)) return;
            try
            {
                dynamic? parsed = JsonConvert.DeserializeObject(payload);
                if (parsed?.policyId != null)
                    state.PolicyId = (int?)parsed.policyId;
                if (parsed?.assetId != null)
                    state.AssetId = (int?)parsed.assetId;
            }
            catch { /* payload enrichment is best-effort */ }
        }

        /// <summary>
        /// Proactively analyzes active assets to identify potential compliance 
        /// risks before they manifest as non-compliant events (e.g., expiring policies).
        /// </summary>
        public async Task RunForecastingAsync()
        {
            using (var db = await _dbFactory.CreateDbContextAsync())
            {
                var activeAssets = await db.Assets
                    .Where(a => a.Status == "Active")
                    .ToListAsync();

                foreach (var asset in activeAssets)
                {
                    var risk = ForecastRisk(db, asset);
                    if (risk != null)
                    {
                        AuditLogger.Log(db,
                            entityName: "Asset",
                            entityId: asset.Id.ToString(),
                            action: "RiskIdentified",
                            newValues: risk,
                            actorName: "InsightEngine",
                            tenantId: asset.TenantId,
                            notes: $"Proactive Risk Identified: {risk.Message}");
                    }
                }
                await db.SaveChangesAsync();
            }
        }

        private dynamic? ForecastRisk(ApplicationDbContext db, Asset asset)
        {
            var activePolicies = db.Policies
                .Where(p => p.AssetId == asset.Id && p.Status == "Active")
                .ToList();

            foreach (var policy in activePolicies)
            {
                var daysToExpiry = (policy.ExpiryDate - DateTime.UtcNow).TotalDays;
                if (daysToExpiry > 0 && daysToExpiry <= 30)
                {
                    return new { 
                        RiskType = "ExpiringPolicy", 
                        Severity = daysToExpiry <= 7 ? "High" : "Medium",
                        Message = $"Policy {policy.PolicyNumber} expires in {Math.Ceiling(daysToExpiry)} days.",
                        AssetIdentifier = asset.AssetIdentifier
                    };
                }
            }
            return null;
        }
    }
}







