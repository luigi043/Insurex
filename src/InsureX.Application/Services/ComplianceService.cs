using InsureX.Application.DTOs;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Application.Services;

/// <summary>
/// Evaluates compliance rules per asset and updates the ComplianceState projection.
/// When non-compliance is detected, a NonComplianceCase is automatically opened.
/// </summary>
public class ComplianceService
{
    private readonly IInsureXDbContext _db;

    public ComplianceService(IInsureXDbContext db)
    {
        _db = db;
    }

    public async Task EvaluateAsync(Guid assetId, Guid tenantId)
    {
        var asset = await _db.Assets
            .Include(a => a.Policies)
            .Include(a => a.ComplianceState)
            .FirstOrDefaultAsync(a => a.Id == assetId);

        if (asset == null) return;

        var (status, reasonCode, reasonDetail) = RunRules(asset);

        // Update or create state
        if (asset.ComplianceState == null)
        {
            asset.ComplianceState = new ComplianceState { TenantId = tenantId, AssetId = assetId };
            _db.ComplianceStates.Add(asset.ComplianceState);
        }

        var prev = asset.ComplianceState.Status;
        asset.ComplianceState.Status = status;
        asset.ComplianceState.ReasonCode = reasonCode;
        asset.ComplianceState.ReasonDetail = reasonDetail;
        asset.ComplianceState.LastEvaluatedUtc = DateTime.UtcNow;

        if (status == ComplianceStatus.NonCompliant && prev != ComplianceStatus.NonCompliant)
            asset.ComplianceState.NonCompliantSinceUtc = DateTime.UtcNow;

        if (status == ComplianceStatus.Compliant && prev == ComplianceStatus.NonCompliant)
            asset.ComplianceState.RestoredUtc = DateTime.UtcNow;

        // Open a case if newly non-compliant
        if (status == ComplianceStatus.NonCompliant && prev != ComplianceStatus.NonCompliant)
        {
            OpenCase(assetId, tenantId, reasonCode, reasonDetail);
        }

        await _db.SaveChangesAsync();
    }

    private static (ComplianceStatus status, string code, string detail) RunRules(Asset asset)
    {
        var activePolicy = asset.Policies
            .Where(p => p.Status == PolicyStatus.Active && p.PremiumPaid)
            .OrderByDescending(p => p.EffectiveDate)
            .FirstOrDefault();

        if (activePolicy == null)
            return (ComplianceStatus.NonCompliant, "NO_ACTIVE_POLICY", "No active, premium-paid policy found for this asset.");

        if (activePolicy.ExpiryDate.HasValue && activePolicy.ExpiryDate.Value < DateTime.UtcNow)
            return (ComplianceStatus.NonCompliant, "POLICY_EXPIRED", $"Policy {activePolicy.PolicyNumber} has expired.");

        if (activePolicy.SumInsured < asset.LoanAmount)
            return (ComplianceStatus.NonCompliant, "UNDERINSURED", $"Sum insured {activePolicy.SumInsured:C} is below loan amount {asset.LoanAmount:C}.");

        return (ComplianceStatus.Compliant, string.Empty, string.Empty);
    }

    private void OpenCase(Guid assetId, Guid tenantId, string reasonCode, string detail)
    {
        var caseNumber = $"NC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
        var newCase = new NonComplianceCase
        {
            TenantId = tenantId,
            AssetId = assetId,
            CaseNumber = caseNumber,
            Status = CaseStatus.Open,
            ReasonCode = reasonCode,
            Description = detail,
            SlaDeadlineUtc = DateTime.UtcNow.AddDays(5)
        };
        _db.Cases.Add(newCase);
    }

    public async Task<PagedResult<ComplianceStateDto>> GetComplianceListAsync(PageRequest req, ComplianceStatus? statusFilter = null)
    {
        var query = _db.ComplianceStates.AsQueryable();
        if (statusFilter.HasValue)
            query = query.Where(c => c.Status == statusFilter.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.LastEvaluatedUtc)
            .Skip((req.ValidPage - 1) * req.ValidPageSize)
            .Take(req.ValidPageSize)
            .Select(c => new ComplianceStateDto
            {
                AssetId = c.AssetId,
                Status = c.Status.ToString(),
                ReasonCode = c.ReasonCode,
                ReasonDetail = c.ReasonDetail,
                LastEvaluatedUtc = c.LastEvaluatedUtc,
                NonCompliantSinceUtc = c.NonCompliantSinceUtc
            })
            .ToListAsync();

        return new PagedResult<ComplianceStateDto> { Items = items, Page = req.ValidPage, PageSize = req.ValidPageSize, TotalItems = total };
    }
}
