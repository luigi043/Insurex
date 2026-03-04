using InsureX.Application.DTOs;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Application.Services;

public class PolicyService
{
    private readonly IInsureXDbContext _db;
    private readonly ComplianceService _complianceService;

    public PolicyService(IInsureXDbContext db, ComplianceService complianceService)
    {
        _db = db;
        _complianceService = complianceService;
    }

    public async Task<PagedResult<PolicyDto>> GetPoliciesAsync(Guid assetId, PageRequest req)
    {
        var query = _db.Policies
            .Include(p => p.InsurerOrg)
            .Where(p => p.AssetId == assetId);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.EffectiveDate)
            .Skip((req.ValidPage - 1) * req.ValidPageSize)
            .Take(req.ValidPageSize)
            .Select(p => new PolicyDto
            {
                Id = p.Id,
                AssetId = p.AssetId,
                PolicyNumber = p.PolicyNumber,
                Status = p.Status.ToString(),
                CoverType = p.CoverType.ToString(),
                SumInsured = p.SumInsured,
                PremiumPaid = p.PremiumPaid,
                EffectiveDate = p.EffectiveDate,
                ExpiryDate = p.ExpiryDate,
                InsurerName = p.InsurerOrg.Name
            })
            .ToListAsync();

        return new PagedResult<PolicyDto> { Items = items, Page = req.ValidPage, PageSize = req.ValidPageSize, TotalItems = total };
    }

    public async Task<PolicyDto> CreateAsync(CreatePolicyRequest req, Guid tenantId)
    {
        var policy = new Policy
        {
            TenantId = tenantId,
            AssetId = req.AssetId,
            InsurerOrgId = req.InsurerOrgId,
            PolicyNumber = req.PolicyNumber,
            CoverType = Enum.Parse<CoverType>(req.CoverType),
            SumInsured = req.SumInsured,
            PremiumPaid = req.PremiumPaid,
            EffectiveDate = req.EffectiveDate,
            ExpiryDate = req.ExpiryDate,
            RiskAddress = req.RiskAddress,
            Status = PolicyStatus.Active
        };

        _db.Policies.Add(policy);
        await _db.SaveChangesAsync();

        // Re-evaluate compliance
        await _complianceService.EvaluateAsync(req.AssetId, tenantId);

        var org = await _db.Organisations.FindAsync(req.InsurerOrgId);
        return new PolicyDto
        {
            Id = policy.Id,
            AssetId = policy.AssetId,
            PolicyNumber = policy.PolicyNumber,
            Status = policy.Status.ToString(),
            CoverType = policy.CoverType.ToString(),
            SumInsured = policy.SumInsured,
            PremiumPaid = policy.PremiumPaid,
            EffectiveDate = policy.EffectiveDate,
            ExpiryDate = policy.ExpiryDate,
            InsurerName = org?.Name ?? string.Empty
        };
    }
}
