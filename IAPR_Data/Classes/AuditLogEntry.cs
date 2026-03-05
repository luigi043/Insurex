using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Immutable audit log entry. Once written, rows are NEVER updated or deleted.
    /// Captures who changed what, when, with old and new values in JSON.
    /// </summary>
    public class AuditLogEntry
    {
        [Key]
        public long Id { get; set; }

        /// <summary>Distributed trace ID linking this entry to a root cause event.</summary>
        [StringLength(100)]
        public string CorrelationId { get; set; }

        /// <summary>EF entity / table name (e.g., "ComplianceState", "Case", "WebhookEvent").</summary>
        [Required]
        [StringLength(100)]
        public string EntityName { get; set; }

        /// <summary>Primary key of the affected entity row.</summary>
        [StringLength(100)]
        public string EntityId { get; set; }

        /// <summary>Action performed: Created | Updated | Deleted | Evaluated | Escalated | Resolved</summary>
        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        /// <summary>JSON snapshot of the entity before the change (null for Created actions).</summary>
        public string OldValues { get; set; }

        /// <summary>JSON snapshot of the entity after the change (null for Deleted actions).</summary>
        public string NewValues { get; set; }

        /// <summary>ASP.NET Identity UserId of the actor (null for system actions).</summary>
        [StringLength(128)]
        public string ActorUserId { get; set; }

        /// <summary>Display name of the actor (denormalised for readability).</summary>
        [StringLength(200)]
        public string ActorName { get; set; }

        /// <summary>Tenant scope.</summary>
        public int? TenantId { get; set; }

        /// <summary>UTC timestamp — set once, immutable.</summary>
        public DateTime OccurredAt { get; set; }

        /// <summary>Optional free-text context (e.g., compliance rule that fired).</summary>
        [StringLength(500)]
        public string Notes { get; set; }

        public AuditLogEntry()
        {
            OccurredAt = DateTime.UtcNow;
        }
    }
}







