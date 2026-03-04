using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace IAPR_Data.Classes
{
    /// <summary>Lifecycle states a compliance case can be in.</summary>
    public enum CaseStatus
    {
        Open,
        InProgress,
        Escalated,
        Resolved,
        Closed
    }

    /// <summary>Priority levels that drive SLA deadline calculation.</summary>
    public enum CasePriority
    {
        Low,      // 7 days
        Medium,   // 72 hours
        High,     // 24 hours
        Critical  // 4 hours
    }

    /// <summary>
    /// A compliance case opened automatically when the ComplianceEngine emits
    /// a NonCompliant or PendingReview outcome. Tracks SLA deadlines and escalation.
    /// </summary>
    public class Case
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Human-readable case number: CASE-{year}-{5-digit-seq}.</summary>
        [Required]
        [StringLength(30)]
        public string CaseNumber { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; }

        [Required]
        [StringLength(20)]
        public string Priority { get; set; }

        /// <summary>FK to the ComplianceState that triggered this case.</summary>
        public int? SourceComplianceStateId { get; set; }

        /// <summary>Identity UserId of the assigned agent (null = unassigned).</summary>
        [StringLength(128)]
        public string AssignedToUserId { get; set; }

        public int? TenantId { get; set; }

        [StringLength(100)]
        public string CorrelationId { get; set; }

        /// <summary>UTC timestamp the case was opened.</summary>
        public DateTime OpenedAt { get; set; }

        /// <summary>SLA deadline: if not resolved by this time the case is auto-escalated.</summary>
        public DateTime DueAt { get; set; }

        /// <summary>UTC timestamp the case was resolved (null if still open).</summary>
        public DateTime? ResolvedAt { get; set; }

        /// <summary>UTC timestamp the case was escalated (null if not escalated).</summary>
        public DateTime? EscalatedAt { get; set; }

        /// <summary>Thread of notes attached to this case.</summary>
        public virtual ICollection<CaseNote> Notes { get; set; } = new List<CaseNote>();

        public Case()
        {
            OpenedAt = DateTime.UtcNow;
            Status = CaseStatus.Open.ToString();
        }
    }

    /// <summary>
    /// An immutable note appended to a compliance case.
    /// System-generated notes (escalation, resolution) are flagged with IsSystemGenerated=true.
    /// </summary>
    public class CaseNote
    {
        [Key]
        public int Id { get; set; }

        public int CaseId { get; set; }

        [ForeignKey("CaseId")]
        public virtual Case Case { get; set; }

        [StringLength(128)]
        public string AuthorUserId { get; set; }

        [StringLength(200)]
        public string AuthorName { get; set; }

        [Required]
        public string NoteText { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>True for notes written by the system (escalation, SLA breach, etc.).</summary>
        public bool IsSystemGenerated { get; set; }

        public CaseNote()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}
