namespace InsureX.Domain.Entities;

public enum CaseStatus { Open, InProgress, PendingInsurer, Escalated, Resolved, Closed }

public class NonComplianceCase
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public CaseStatus Status { get; set; } = CaseStatus.Open;
    public string ReasonCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? AssignedToUserId { get; set; }
    public DateTime OpenedUtc { get; set; } = DateTime.UtcNow;
    public DateTime SlaDeadlineUtc { get; set; }
    public DateTime? ResolvedUtc { get; set; }
    public DateTime? ClosedUtc { get; set; }

    public Asset Asset { get; set; } = null!;
    public ICollection<CaseTask> Tasks { get; set; } = new List<CaseTask>();
    public ICollection<AuditEntry> AuditEntries { get; set; } = new List<AuditEntry>();
}
