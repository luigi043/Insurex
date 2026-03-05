using IAPR_Data.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Api.Controllers;

/// <summary>
/// AssetsController – paginated asset listing and detail.
/// Route: GET /api/assets          (paged list)
///        GET /api/assets/{id}     (single asset + detail)
///        POST /api/assets         (create new asset via EF Core)
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public AssetsController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/assets?page=1&pageSize=10&status=Active&assetType=Motor&registrationNumber=ABC
    [HttpGet]
    public async Task<IActionResult> GetAssets(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? status = null,
        [FromQuery] string? assetType = null,
        [FromQuery] string? registrationNumber = null)
    {
        try
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _db.Assets.AsNoTracking();

            // Optional filters
            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(a => a.Status == status);

            if (!string.IsNullOrWhiteSpace(assetType))
                query = query.Where(a => a.AssetType.Contains(assetType));

            if (!string.IsNullOrWhiteSpace(registrationNumber))
                query = query.Where(a => a.RegistrationNumber != null &&
                                         a.RegistrationNumber.Contains(registrationNumber));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    id = a.Id,
                    tenantId = a.TenantId,
                    assetType = a.AssetType,
                    assetIdentifier = a.AssetIdentifier,
                    registrationNumber = a.RegistrationNumber,
                    financedAmount = a.FinancedAmount,
                    borrowerReference = a.BorrowerReference,
                    loanStartDate = a.LoanStartDate,
                    loanEndDate = a.LoanEndDate,
                    status = a.Status,
                    complianceStatus = a.ComplianceStatus,
                    createdUtc = a.CreatedAt
                })
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return Ok(new
            {
                items,
                page,
                pageSize,
                totalCount,
                totalPages,
                hasNextPage = page < totalPages,
                hasPreviousPage = page > 1
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // GET /api/assets/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsset(int id)
    {
        try
        {
            var asset = await _db.Assets
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    id = a.Id,
                    tenantId = a.TenantId,
                    assetType = a.AssetType,
                    assetIdentifier = a.AssetIdentifier,
                    registrationNumber = a.RegistrationNumber,
                    financedAmount = a.FinancedAmount,
                    borrowerReference = a.BorrowerReference,
                    loanStartDate = a.LoanStartDate,
                    loanEndDate = a.LoanEndDate,
                    status = a.Status,
                    complianceStatus = a.ComplianceStatus,
                    createdUtc = a.CreatedAt,
                    // Nested detail stubs – extend when related tables are joined
                    borrower = new
                    {
                        name = a.BorrowerReference ?? "Unknown",
                        idNumber = "",
                        email = "",
                        phone = ""
                    },
                    policies = new object[] { },
                    complianceHistory = new object[] { }
                })
                .FirstOrDefaultAsync();

            if (asset == null)
                return NotFound(new { message = $"Asset {id} not found." });

            return Ok(new { success = true, data = asset });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    // POST /api/assets  – create asset via EF Core (new path alongside legacy /api/asset)
    [HttpPost]
    public async Task<IActionResult> CreateAsset([FromBody] CreateAssetRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.AssetIdentifier))
                return BadRequest(new { message = "AssetIdentifier is required." });

            var asset = new Asset
            {
                AssetType = request.AssetType ?? "Motor",
                AssetIdentifier = request.AssetIdentifier,
                RegistrationNumber = request.RegistrationNumber ?? "",
                FinancedAmount = request.FinancedAmount,
                BorrowerReference = request.BorrowerReference ?? "",
                LoanStartDate = request.LoanStartDate == default ? DateTime.UtcNow : request.LoanStartDate,
                LoanEndDate = request.LoanEndDate == default ? DateTime.UtcNow.AddYears(5) : request.LoanEndDate,
                Status = "Active",
                ComplianceStatus = "Pending",
                TenantId = request.TenantId
            };

            _db.Assets.Add(asset);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, new
            {
                success = true,
                message = "Asset registered successfully.",
                data = new { id = asset.Id }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

public class CreateAssetRequest
{
    public string? AssetType { get; set; }
    public string AssetIdentifier { get; set; } = "";
    public string? RegistrationNumber { get; set; }
    public decimal FinancedAmount { get; set; }
    public string? BorrowerReference { get; set; }
    public DateTime LoanStartDate { get; set; }
    public DateTime LoanEndDate { get; set; }
    public int? TenantId { get; set; }
}