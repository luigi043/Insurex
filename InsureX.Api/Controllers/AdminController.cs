using IAPR_Data.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Api.Controllers;

[Authorize(Roles = "Administrator")]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(
        ApplicationDbContext db, 
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _db = db;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // GET /api/admin/users
    [HttpGet("users")]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _userManager.Users;
        var total = await query.CountAsync();
        
        var users = await query
            .OrderBy(u => u.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var results = new List<UserDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            results.Add(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = $"{user.vcName} {user.vcSurname}",
                TenantId = user.TenantId,
                Role = roles.FirstOrDefault() ?? "User",
                IsActive = user.iUser_Status_Id == 1
            });
        }

        return Ok(new PagedResult<UserDto>(results, total, page, pageSize));
    }

    // POST /api/admin/users
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] UserDto model)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                vcName = model.FullName?.Split(' ').FirstOrDefault(),
                vcSurname = model.FullName?.Split(' ').LastOrDefault(),
                TenantId = model.TenantId,
                iUser_Status_Id = 1, // Active
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, "Password123!"); // Default password for migration
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!string.IsNullOrEmpty(model.Role))
            {
                if (!await _roleManager.RoleExistsAsync(model.Role))
                    await _roleManager.CreateAsync(new IdentityRole(model.Role));
                
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { message = "User created successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // PUT /api/admin/users/{id}
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto model)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.UserName = model.UserName;
            user.vcName = model.FullName?.Split(' ').FirstOrDefault();
            user.vcSurname = model.FullName?.Split(' ').LastOrDefault();
            user.TenantId = model.TenantId;
            user.iUser_Status_Id = model.IsActive ? 1 : 2;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { message = "User updated successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
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
            Identifier = t.DomainKey,
            Type = "Financer", // Defaulting for now
            CreatedAt = t.CreatedAt,
            IsActive = t.IsActive
        }).ToList();

        return Ok(new PagedResult<TenantDto>(results, total, page, pageSize));
    }

    // POST /api/admin/tenants
    [HttpPost("tenants")]
    public async Task<IActionResult> CreateTenant([FromBody] TenantDto model)
    {
        try
        {
            var tenant = new Tenant
            {
                Name = model.Name,
                DomainKey = model.Identifier,
                Type = model.Type,
                IsActive = true
            };

            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Tenant created successfully", tenantId = tenant.Id });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // POST /api/admin/tenants/financer
    [HttpPost("tenants/financer")]
    public async Task<IActionResult> CreateFinancer([FromBody] TenantDto model)
    {
        model.Type = "Financer";
        return await CreateTenant(model);
    }

    // POST /api/admin/tenants/insurer
    [HttpPost("tenants/insurer")]
    public async Task<IActionResult> CreateInsurer([FromBody] TenantDto model)
    {
        model.Type = "Insurer";
        return await CreateTenant(model);
    }

    // PUT /api/admin/tenants/{id}
    [HttpPut("tenants/{id}")]
    public async Task<IActionResult> UpdateTenant(int id, [FromBody] TenantDto model)
    {
        try
        {
            var tenant = await _db.Tenants.FindAsync(id);
            if (tenant == null) return NotFound();

            tenant.Name = model.Name;
            tenant.DomainKey = model.Identifier;
            tenant.Type = model.Type;
            tenant.IsActive = model.IsActive;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Tenant updated successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }
}
