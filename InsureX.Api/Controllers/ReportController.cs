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
        var query = _db.AuditLog.AsQueryable();

        if (tenantId.HasValue)
            query = query.Where(a => a.TenantId == tenantId);

        var logs = await query.OrderByDescending(a => a.OccurredAt).Take(1000).ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("Timestamp,Entity,Action,Actor,CorrelationId,Notes");

        foreach (var log in logs)
        {
            csv.AppendLine($"{log.OccurredAt:yyyy-MM-dd HH:mm:ss},{log.EntityName},{log.Action},{log.ActorName},{log.CorrelationId},\"{log.Notes?.Replace("\"", "'")}\"");
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

    [HttpGet("users/export")]
    public async Task<IActionResult> GetUsersExport()
    {
        // Join Users with UserRoles and Roles to get the Role Name
        var usersWithRoles = await (from user in _db.Users
                                    join userRole in _db.UserRoles on user.Id equals userRole.UserId into ur
                                    from userRole in ur.DefaultIfEmpty()
                                    join role in _db.Roles on userRole.RoleId equals role.Id into r
                                    from role in r.DefaultIfEmpty()
                                    select new 
                                    {
                                        user.LegacyUserId,
                                        user.UserName,
                                        user.vcName,
                                        user.vcSurname,
                                        user.vcPosition_Title,
                                        user.Email,
                                        user.iUser_Status_Id,
                                        RoleName = role != null ? role.Name : "None"
                                    }).ToListAsync();

        var csv = new StringBuilder();
        csv.AppendLine("LegacyUserId,UserName,Name,Surname,Position,Email,IsActive,Role,Password_Warning");

        foreach (var user in usersWithRoles)
        {
            csv.AppendLine($"{user.LegacyUserId},{user.UserName},\"{user.vcName}\",\"{user.vcSurname}\",\"{user.vcPosition_Title}\",{user.Email},{(user.iUser_Status_Id == 1 ? "Active" : "Inactive")},\"{user.RoleName}\",\"PROTECTED_BY_BCRYPT_HASH_CANNOT_EXPORT\"");
        }

        return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"Users_Export_{DateTime.UtcNow:yyyyMMdd}.csv");
    }

    private int? GetTenantId()
    {
        var claim = User.FindFirst("TenantId");
        if (claim != null && int.TryParse(claim.Value, out int tid))
            return tid;
        return null;
    }
}
