namespace InsureX.Domain.Entities;

public enum ComplianceStatus { Compliant, NonCompliant, Pending, Unknown }

public class ComplianceState
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public ComplianceStatus Status { get; set; } = ComplianceStatus.Unknown;
    public string ReasonCode { get; set; } = string.Empty;
    public string ReasonDetail { get; set; } = string.Empty;
    public DateTime LastEvaluatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? NonCompliantSinceUtc { get; set; }
    public DateTime? RestoredUtc { get; set; }

    public Asset Asset { get; set; } = null!;
}
