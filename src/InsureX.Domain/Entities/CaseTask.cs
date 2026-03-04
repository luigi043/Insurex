namespace InsureX.Domain.Entities;

public enum TaskStatus { Pending, InProgress, Done, Cancelled }
public enum TaskType { SendSms, SendEmail, CallCustomer, EscalateToBank, EscalateToInsurer, ManualReview }

public class CaseTask
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid CaseId { get; set; }
    public TaskType Type { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;
    public string Notes { get; set; } = string.Empty;
    public Guid? AssignedToUserId { get; set; }
    public DateTime DueUtc { get; set; }
    public DateTime? CompletedUtc { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public NonComplianceCase Case { get; set; } = null!;
}
