using System;
using System.ComponentModel.DataAnnotations;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Enumerates the possible compliance outcomes emitted by the ComplianceEngine.
    /// </summary>
    public enum ComplianceOutcome
    {
        /// <summary>The event was processed and the asset/policy is compliant.</summary>
        Compliant,

        /// <summary>The event reveals a compliance breach; remediation action required.</summary>
        NonCompliant,

        /// <summary>The event could not be classified — manual review needed.</summary>
        PendingReview,

        /// <summary>The event was ignored (e.g., unknown type, duplicate, irrelevant source).</summary>
        Ignored
    }

    /// <summary>
    /// Persisted record of a compliance evaluation result.
    /// Emitted by the <see cref="ComplianceEngine"/> for every processed webhook event.
    /// </summary>
    public class ComplianceState
    {
        [Key]
        public int Id { get; set; }

        /// <summary>The EventId of the source webhook event that triggered this evaluation.</summary>
        [Required]
        [StringLength(200)]
        public string SourceEventId { get; set; }

        /// <summary>The compliance outcome determined by the engine.</summary>
        [Required]
        [StringLength(50)]
        public string Outcome { get; set; }

        /// <summary>Human-readable reason/rule that produced this outcome.</summary>
        [StringLength(1000)]
        public string Reason { get; set; }

        /// <summary>Optional reference to the related Policy record.</summary>
        public int? PolicyId { get; set; }

        /// <summary>Optional reference to the related Asset (any type) record.</summary>
        public int? AssetId { get; set; }

        /// <summary>Tenant scope.</summary>
        public int? TenantId { get; set; }

        /// <summary>Correlation ID for distributed tracing across the system.</summary>
        [StringLength(100)]
        public string CorrelationId { get; set; }

        /// <summary>UTC timestamp of the evaluation.</summary>
        public DateTime EvaluatedAt { get; set; }

        public ComplianceState()
        {
            EvaluatedAt = DateTime.UtcNow;
            CorrelationId = Guid.NewGuid().ToString("N");
        }
    }
}
