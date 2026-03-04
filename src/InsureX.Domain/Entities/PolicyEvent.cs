namespace InsureX.Domain.Entities;

public enum EventType
{
    PolicyCancelled,
    PremiumPaymentFailed,
    CoverageReduced,
    RiskAddressChanged,
    AssetRemovedFromCover,
    PolicyRenewed,
    PolicyConfirmed,
    RegistrationChanged
}

public class PolicyEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid PolicyId { get; set; }
    public Guid AssetId { get; set; }
    public EventType EventType { get; set; }
    public string SourceSystem { get; set; } = string.Empty;     // insurer code
    public string SourceEventId { get; set; } = string.Empty;    // idempotency
    public string RawPayload { get; set; } = string.Empty;       // JSON blob
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime OccurredUtc { get; set; } = DateTime.UtcNow;

    public Policy Policy { get; set; } = null!;
}
