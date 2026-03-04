namespace InsureX.Domain.Entities;

public enum AssetType { MotorVehicle, NonMotor }
public enum AssetStatus { Active, Settled, Closed }

public class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid OrgId { get; set; }                       // owning bank org
    public string VIN { get; set; } = string.Empty;       // vehicle ID / serial
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AssetType AssetType { get; set; }
    public AssetStatus Status { get; set; } = AssetStatus.Active;
    public string BorrowerRef { get; set; } = string.Empty;
    public string FacilityRef { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public DateTime LoanInceptionDate { get; set; }
    public DateTime LoanMaturityDate { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedUtc { get; set; }

    public Organisation Organisation { get; set; } = null!;
    public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    public ComplianceState? ComplianceState { get; set; }
    public ICollection<NonComplianceCase> Cases { get; set; } = new List<NonComplianceCase>();
}
