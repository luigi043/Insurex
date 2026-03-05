using IAPR_Data.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ReportController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("audit/export")]
    public async Task<IActionResult> GetAuditExport()
    {
        var tenantId = GetTenantId();
        var query = _db.AuditLogEntries.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(a => a.TenantId == tenantId);

        var logs = await query.OrderByDescending(a => a.Timestamp).Take(1000).ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Timestamp,Entity,Action,Actor,CorrelationId,Notes");

        foreach (var log in logs)
        {
            csv.AppendLine($"{log.Timestamp:yyyy-MM-dd HH:mm:ss},{log.EntityName},{log.Action},{log.ActorName},{log.CorrelationId},\"{log.Notes?.Replace("\"", "'")}\"");
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"AuditLog_Export_{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    [HttpGet("assets/export")]
    public async Task<IActionResult> GetAssetsExport()
    {
        var tenantId = GetTenantId();
        var query = _db.Assets.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(a => a.TenantId == tenantId);

        var assets = await query.ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Identifier,Type,Status,ComplianceStatus,RegistrationNumber,Borrower");

        foreach (var asset in assets)
        {
            csv.AppendLine($"{asset.AssetIdentifier},{asset.AssetType},{asset.Status},{asset.ComplianceStatus},{asset.RegistrationNumber},{asset.BorrowerReference}");
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"Assets_Portfolio_Export_{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    private int? GetTenantId()
    {
        var claim = User.FindFirst("TenantId");
        if (claim != null && int.TryParse(claim.Value, out int tid))
            return tid;
        return null;
    }
}
