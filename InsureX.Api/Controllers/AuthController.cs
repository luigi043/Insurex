using IAPR_Data.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;

    public AuthController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // ── Step 1: Try legacy provider (only works if legacy DB connection is configured) ──
        try
        {
            var userProv = new P.User_Provider();
            var legacyUser = userProv.ValidateUser(request.UserName, request.Password);

            if (legacyUser != null)
            {
                var token = GenerateJwtToken(
                    legacyUser.iUser_Id.ToString(),
                    request.UserName,
                    legacyUser.vcName ?? request.UserName,
                    legacyUser.iUser_Type_Id.ToString(),
                    legacyUser.iPartner_Id.ToString()
                );

                return Ok(new LoginResponse
                {
                    Token = token,
                    UserName = request.UserName,
                    FullName = legacyUser.vcName ?? "",
                    Role = MapRole(legacyUser.iUser_Type_Id),
                    PartnerId = legacyUser.iPartner_Id
                });
            }
        }
        catch
        {
            // Legacy provider not configured — fall through to ASP.NET Identity
        }

        // ── Step 2: ASP.NET Identity login (email or username) ──
        var identityUser = await _userManager.FindByEmailAsync(request.UserName)
                        ?? await _userManager.FindByNameAsync(request.UserName);

        if (identityUser == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var passwordValid = await _userManager.CheckPasswordAsync(identityUser, request.Password);
        if (!passwordValid)
            return Unauthorized(new { message = "Invalid credentials" });

        var roles = await _userManager.GetRolesAsync(identityUser);
        var primaryRole = roles.FirstOrDefault() ?? MapRole(identityUser.iUser_Type_Id);

        var jwtToken = GenerateJwtToken(
            identityUser.Id,
            identityUser.UserName ?? request.UserName,
            $"{identityUser.vcName} {identityUser.vcSurname}".Trim(),
            identityUser.iUser_Type_Id.ToString(),
            (identityUser.iPartner_Id ?? 0).ToString(),
            primaryRole
        );

        return Ok(new LoginResponse
        {
            Token = jwtToken,
            UserName = identityUser.UserName ?? request.UserName,
            FullName = $"{identityUser.vcName} {identityUser.vcSurname}".Trim(),
            Role = primaryRole,
            PartnerId = identityUser.iPartner_Id ?? 0
        });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        return Ok(new { message = "Token refresh not yet implemented" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString)) return Unauthorized("User ID not found in token.");

        var user = await _userManager.FindByIdAsync(userIdString);

        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
                return Ok(new { message = "Password updated successfully." });
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        // Legacy fallback
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(userName)) return Unauthorized();

        try
        {
            var userProv = new P.User_Provider();
            var legacyUser = userProv.ValidateUser(userName, request.CurrentPassword);
            if (legacyUser == null)
                return BadRequest(new { errors = new[] { "Incorrect current password." } });

            var success = userProv.ChangePassword(legacyUser.iUser_Id, userName, request.NewPassword);
            if (success)
                return Ok(new { message = "Password updated successfully via legacy provider." });

            return BadRequest(new { errors = new[] { "Failed to update password." } });
        }
        catch
        {
            return BadRequest(new { errors = new[] { "Legacy provider unavailable." } });
        }
    }

    private string GenerateJwtToken(string userId, string userName, string fullName,
        string userTypeId, string partnerId, string? role = null)
    {
        var jwtConfig = _config.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]
            ?? "Generic_Secret_Key_32bytes_Required_For_Production_HS256");

        var resolvedRole = role ?? MapRole(int.TryParse(userTypeId, out var t) ? t : 0);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(ClaimTypes.Name, userName),
            new Claim("vcName", fullName),
            new Claim("iUser_Type_Id", userTypeId),
            new Claim("iPartner_Id", partnerId),
            new Claim(ClaimTypes.Role, resolvedRole),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtConfig["Issuer"],
            audience: jwtConfig["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string MapRole(int userTypeId) => userTypeId switch
    {
        1 or 2 => "Administrator",
        3 or 4 => "Manager",
        5 or 6 => "InsurerUser",
        _ => "User"
    };
}

public class LoginRequest
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}

public class LoginResponse
{
    public string Token { get; set; } = "";
    public string UserName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
    public int PartnerId { get; set; }
}

public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; } = "";
    public string NewPassword { get; set; } = "";
}