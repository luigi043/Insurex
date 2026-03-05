using IAPR_Data.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Api.Controllers;

[Authorize(Roles = "Administrator")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public AdminController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET /api/admin/users
    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        // Mocking user retrieval for now as Identity migration is in progress
        var users = new List<UserDto>
        {
            new UserDto { Id = "1", UserName = "admin@insurex.com", Email = "admin@insurex.com", FullName = "System Admin", Role = "Administrator", IsActive = true },
            new UserDto { Id = "2", UserName = "banker@testbank.com", Email = "banker@testbank.com", FullName = "James Banker", TenantId = 1, Role = "Manager", IsActive = true }
        };

        return Ok(new PagedResult<UserDto>(users, users.Count, page, pageSize));
    }

    // GET /api/admin/tenants
    [HttpGet("tenants")]
    public async Task<ActionResult<PagedResult<TenantDto>>> GetTenants([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var total = await _db.Tenants.CountAsync();
        var tenants = await _db.Tenants
            .OrderBy(t => t.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var results = tenants.Select(t => new TenantDto
        {
            Id = t.Id,
            Name = t.Name,
            Identifier = t.Identifier,
            Type = "Financer",
            CreatedAt = DateTime.UtcNow.AddYears(-1),
            IsActive = t.IsActive
        }).ToList();

        return Ok(new PagedResult<TenantDto>(results, total, page, pageSize));
    }
}
