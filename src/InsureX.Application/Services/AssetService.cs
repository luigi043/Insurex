using InsureX.Application.DTOs;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Application.Services;

public class AssetService
{
    private readonly IInsureXDbContext _db;

    public AssetService(IInsureXDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<AssetDto>> GetAssetsAsync(PageRequest req, string? vinFilter = null, AssetStatus? statusFilter = null)
    {
        var query = _db.Assets
            .Include(a => a.ComplianceState)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(vinFilter))
            query = query.Where(a => a.VIN.Contains(vinFilter));

        if (statusFilter.HasValue)
            query = query.Where(a => a.Status == statusFilter.Value);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.CreatedUtc)
            .Skip((req.ValidPage - 1) * req.ValidPageSize)
            .Take(req.ValidPageSize)
            .Select(a => new AssetDto
            {
                Id = a.Id,
                VIN = a.VIN,
                RegistrationNumber = a.RegistrationNumber,
                Description = a.Description,
                AssetType = a.AssetType.ToString(),
                Status = a.Status.ToString(),
                BorrowerRef = a.BorrowerRef,
                FacilityRef = a.FacilityRef,
                LoanAmount = a.LoanAmount,
                LoanInceptionDate = a.LoanInceptionDate,
                LoanMaturityDate = a.LoanMaturityDate,
                ComplianceStatus = a.ComplianceState != null ? a.ComplianceState.Status.ToString() : "Unknown",
                CreatedUtc = a.CreatedUtc
            })
            .ToListAsync();

        return new PagedResult<AssetDto>
        {
            Items = items,
            Page = req.ValidPage,
            PageSize = req.ValidPageSize,
            TotalItems = total
        };
    }

    public async Task<AssetDto?> GetByIdAsync(Guid id)
    {
        var a = await _db.Assets.Include(x => x.ComplianceState).FirstOrDefaultAsync(x => x.Id == id);
        if (a == null) return null;

        return new AssetDto
        {
            Id = a.Id,
            VIN = a.VIN,
            RegistrationNumber = a.RegistrationNumber,
            Description = a.Description,
            AssetType = a.AssetType.ToString(),
            Status = a.Status.ToString(),
            BorrowerRef = a.BorrowerRef,
            FacilityRef = a.FacilityRef,
            LoanAmount = a.LoanAmount,
            LoanInceptionDate = a.LoanInceptionDate,
            LoanMaturityDate = a.LoanMaturityDate,
            ComplianceStatus = a.ComplianceState?.Status.ToString() ?? "Unknown",
            CreatedUtc = a.CreatedUtc
        };
    }

    public async Task<AssetDto> CreateAsync(CreateAssetRequest req, Guid tenantId)
    {
        var asset = new Asset
        {
            TenantId = tenantId,
            OrgId = req.OrgId,
            VIN = req.VIN,
            RegistrationNumber = req.RegistrationNumber,
            Description = req.Description,
            AssetType = Enum.Parse<AssetType>(req.AssetType),
            BorrowerRef = req.BorrowerRef,
            FacilityRef = req.FacilityRef,
            LoanAmount = req.LoanAmount,
            LoanInceptionDate = req.LoanInceptionDate,
            LoanMaturityDate = req.LoanMaturityDate,
            Status = AssetStatus.Active
        };

        // create default compliance state
        asset.ComplianceState = new ComplianceState
        {
            TenantId = tenantId,
            AssetId = asset.Id,
            Status = ComplianceStatus.Unknown
        };

        _db.Assets.Add(asset);
        await _db.SaveChangesAsync();

        return (await GetByIdAsync(asset.Id))!;
    }
}
