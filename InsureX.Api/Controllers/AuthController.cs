using IAPR_Data.Classes;
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
        try
        {
            // Try legacy provider first
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

            return Unauthorized(new { message = "Invalid credentials" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        // Placeholder for token refresh
        return Ok(new { message = "Token refresh not yet implemented" });
    }

    private string GenerateJwtToken(string userId, string userName, string fullName, string userTypeId, string partnerId)
    {
        var jwtConfig = _config.GetSection("Jwt");
        var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]
            ?? "Generic_Secret_Key_32bytes_Required_For_Production_HS256");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim("vcName", fullName),
            new Claim("iUser_Type_Id", userTypeId),
            new Claim("iPartner_Id", partnerId),
            new Claim(ClaimTypes.Role, MapRole(int.Parse(userTypeId))),
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
