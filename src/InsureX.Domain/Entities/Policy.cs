namespace InsureX.Domain.Entities;

public enum PolicyStatus { Active, Lapsed, Cancelled, PendingRenewal, Unknown }
public enum CoverType { Comprehensive, ThirdParty, FireAndTheft }

public class Policy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid AssetId { get; set; }
    public Guid InsurerOrgId { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public PolicyStatus Status { get; set; } = PolicyStatus.Unknown;
    public CoverType CoverType { get; set; }
    public decimal SumInsured { get; set; }
    public bool PremiumPaid { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string RiskAddress { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; set; }

    public Asset Asset { get; set; } = null!;
    public Organisation InsurerOrg { get; set; } = null!;
    public ICollection<PolicyEvent> Events { get; set; } = new List<PolicyEvent>();
}
