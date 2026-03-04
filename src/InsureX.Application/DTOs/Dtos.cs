namespace InsureX.Application.DTOs;

// ── Paging ────────────────────────────────────────────────────────────────────
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    public bool HasNext => Page < TotalPages;
}

public class PageRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public string? SortBy { get; set; }
    public string SortDir { get; set; } = "asc";

    public int ValidPageSize => Math.Min(Math.Max(PageSize, 1), 100);
    public int ValidPage => Math.Max(Page, 1);
}

// ── Asset DTOs ────────────────────────────────────────────────────────────────
public class AssetDto
{
    public Guid Id { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string BorrowerRef { get; set; } = string.Empty;
    public string FacilityRef { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public DateTime LoanInceptionDate { get; set; }
    public DateTime LoanMaturityDate { get; set; }
    public string ComplianceStatus { get; set; } = "Unknown";
    public DateTime CreatedUtc { get; set; }
}

public class CreateAssetRequest
{
    public string VIN { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AssetType { get; set; } = "MotorVehicle";
    public string BorrowerRef { get; set; } = string.Empty;
    public string FacilityRef { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public DateTime LoanInceptionDate { get; set; }
    public DateTime LoanMaturityDate { get; set; }
    public Guid OrgId { get; set; }
}

// ── Policy DTOs ───────────────────────────────────────────────────────────────
public class PolicyDto
{
    public Guid Id { get; set; }
    public Guid AssetId { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CoverType { get; set; } = string.Empty;
    public decimal SumInsured { get; set; }
    public bool PremiumPaid { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string InsurerName { get; set; } = string.Empty;
}

public class CreatePolicyRequest
{
    public Guid AssetId { get; set; }
    public Guid InsurerOrgId { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public string CoverType { get; set; } = "Comprehensive";
    public decimal SumInsured { get; set; }
    public bool PremiumPaid { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string RiskAddress { get; set; } = string.Empty;
}

// ── Compliance DTOs ───────────────────────────────────────────────────────────
public class ComplianceStateDto
{
    public Guid AssetId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ReasonCode { get; set; } = string.Empty;
    public string ReasonDetail { get; set; } = string.Empty;
    public DateTime LastEvaluatedUtc { get; set; }
    public DateTime? NonCompliantSinceUtc { get; set; }
}

// ── Case DTOs ─────────────────────────────────────────────────────────────────
public class CaseDto
{
    public Guid Id { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public Guid AssetId { get; set; }
    public string AssetVin { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ReasonCode { get; set; } = string.Empty;
    public DateTime OpenedUtc { get; set; }
    public DateTime SlaDeadlineUtc { get; set; }
    public DateTime? ResolvedUtc { get; set; }
    public bool SlaBreached => DateTime.UtcNow > SlaDeadlineUtc && ResolvedUtc == null;
}

// ── Insurer Webhook ───────────────────────────────────────────────────────────
public class InsurerWebhookPayload
{
    public string EventType { get; set; } = string.Empty;
    public string SourceEventId { get; set; } = string.Empty;
    public string PolicyNumber { get; set; } = string.Empty;
    public string PolicyStatus { get; set; } = string.Empty;
    public bool PremiumPaid { get; set; }
    public DateTime OccurredAt { get; set; }
    public Dictionary<string, string> AdditionalData { get; set; } = new();
}
