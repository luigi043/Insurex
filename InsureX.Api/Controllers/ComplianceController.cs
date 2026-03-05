using IAPR_Data.Classes;
using IAPR_Data.Services;
using IAPR_Data.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Data;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ComplianceController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly ComplianceEngine _complianceEngine;
    private readonly Daschboard_Provider _dashboardProvider;

    public ComplianceController(ApplicationDbContext db, ComplianceEngine complianceEngine, Daschboard_Provider dashboardProvider)
    {
        _db = db;
        _complianceEngine = complianceEngine;
        _dashboardProvider = dashboardProvider;
    }

    // GET /api/compliance/states
    [HttpGet("states")]
    public async Task<ActionResult<PagedResult<object>>> GetComplianceStates([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? outcome = null)
    {
        var tenantId = GetTenantId();
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.ComplianceStates.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(s => s.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(outcome))
            query = query.Where(s => s.Outcome == outcome);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(s => s.EvaluatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new
            {
                s.Id, s.SourceEventId, s.Outcome, s.Reason,
                s.PolicyId, s.AssetId, s.TenantId, s.CorrelationId, s.EvaluatedAt
            })
            .ToListAsync();

        return Ok(new PagedResult<object>(items.Cast<object>(), total, page, pageSize));
    }

    // GET /api/compliance/cases
    [HttpGet("cases")]
    public async Task<ActionResult<PagedResult<object>>> GetCases([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null, [FromQuery] string? priority = null)
    {
        var tenantId = GetTenantId();
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _db.Cases.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(c => c.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(c => c.Status == status);

        if (!string.IsNullOrWhiteSpace(priority))
            query = query.Where(c => c.Priority == priority);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.OpenedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new
            {
                c.Id, c.CaseNumber, c.Title, c.Status, c.Priority,
                c.AssignedToUserId, c.OpenedAt, c.DueAt, c.ResolvedAt,
                c.EscalatedAt, c.TenantId, c.CorrelationId
            })
            .ToListAsync();

        return Ok(new PagedResult<object>(items.Cast<object>(), total, page, pageSize));
    }

    // GET /api/compliance/cases/{id}
    [HttpGet("cases/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> GetCase(int id)
    {
        var tenantId = GetTenantId();
        var cas = await _db.Cases.FindAsync(id);

        if (cas == null || (tenantId.HasValue && cas.TenantId != tenantId))
            return NotFound(ApiResponse<object>.Fail("Case not found."));

        var notes = await _db.CaseNotes
            .Where(n => n.CaseId == id)
            .OrderBy(n => n.CreatedAt)
            .Select(n => new { n.Id, n.AuthorName, n.NoteText, n.CreatedAt, n.IsSystemGenerated })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(new
        {
            cas.Id, cas.CaseNumber, cas.Title, cas.Description, cas.Status, cas.Priority,
            cas.SourceComplianceStateId, cas.AssignedToUserId, cas.OpenedAt, cas.DueAt,
            cas.ResolvedAt, cas.EscalatedAt, cas.TenantId, cas.CorrelationId,
            Notes = notes
        }));
    }

    // GET /api/compliance/dashboard/summary
    [HttpGet("dashboard/summary")]
    public async Task<ActionResult<ApiResponse<object>>> GetDashboardSummary()
    {
        var tenantId = GetTenantId();
        DataSet ds;

        if (User.IsInRole("Administrator"))
            ds = _dashboardProvider.Get_Admin_Landing_DashboardTable();
        else if (tenantId.HasValue)
            ds = _dashboardProvider.Get_Financer_Landing_DashboardTable(tenantId.Value);
        else
            return Forbid();

        return Ok(ApiResponse<object>.Ok(ds));
    }

    // POST /api/compliance/cases/{id}/resolve
    [HttpPost("cases/{id}/resolve")]
    public async Task<ActionResult<ApiResponse<string>>> ResolveCase(int id, [FromBody] string resolution)
    {
        var cas = await _db.Cases.FindAsync(id);
        if (cas == null) return NotFound(ApiResponse<string>.Fail("Case not found."));

        cas.Status = "Resolved";
        cas.ResolvedAt = DateTime.UtcNow;
        
        _db.CaseNotes.Add(new CaseNote 
        { 
            CaseId = id, 
            AuthorName = User.Identity?.Name ?? "System", 
            NoteText = resolution, 
            CreatedAt = DateTime.UtcNow 
        });

        await _db.SaveChangesAsync();
        return Ok(ApiResponse<string>.Ok("Case resolved successfully."));
    }

    // GET /api/compliance/audit
    [HttpGet("audit")]
    public async Task<ActionResult<PagedResult<AuditLogEntry>>> GetAuditLog([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var tenantId = GetTenantId();
        var query = _db.AuditLog.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(a => a.TenantId == tenantId);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(a => a.OccurredAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResult<AuditLogEntry>(items, total, page, pageSize));
    }

    // GET /api/compliance/assets
    [HttpGet("assets")]
    public async Task<ActionResult<PagedResult<Asset>>> GetAssets([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var tenantId = GetTenantId();
        var query = _db.Assets.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(a => a.TenantId == tenantId);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(a => a.Status == status);

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(a => a.AssetIdentifier)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResult<Asset>(items, total, page, pageSize));
    }

    // GET /api/compliance/assets/{id}
    [HttpGet("assets/{id}")]
    public async Task<ActionResult<ApiResponse<Asset>>> GetAsset(int id)
    {
        var tenantId = GetTenantId();
        var asset = await _db.Assets.FindAsync(id);

        if (asset == null || (tenantId.HasValue && asset.TenantId != tenantId))
            return NotFound(ApiResponse<Asset>.Fail("Asset not found."));

        return Ok(ApiResponse<Asset>.Ok(asset));
    }

    private int? GetTenantId()
    {
        var claim = User.FindFirst("TenantId");
        if (claim != null && int.TryParse(claim.Value, out int tid))
            return tid;
        return null;
    }
}
