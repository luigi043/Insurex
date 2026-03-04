using InsureX.Application.DTOs;
using InsureX.Application.Services;
using InsureX.Application.Interfaces;
using InsureX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsureX.Api.Controllers;

/// <summary>
/// Controller for managing insurance policies.
/// Provides functionality for retrieving and creating policies.
/// </summary>
[ApiController]
[Route("api/v1")]
public class PoliciesController : ControllerBase
{
    private readonly PolicyService _policies;
    private readonly ITenantContext _tenant;

    public PoliciesController(PolicyService policies, ITenantContext tenant)
    {
        _policies = policies;
        _tenant = tenant;
    }

    /// <summary>GET /api/v1/assets/{assetId}/policies</summary>
    [HttpGet("assets/{assetId:guid}/policies")]
    public async Task<ActionResult<PagedResult<PolicyDto>>> GetByAsset(
        Guid assetId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        var req = new PageRequest { Page = page, PageSize = pageSize };
        return Ok(await _policies.GetPoliciesAsync(assetId, req));
    }

    /// <summary>POST /api/v1/policies</summary>
    [HttpPost("policies")]
    public async Task<ActionResult<PolicyDto>> Create([FromBody] CreatePolicyRequest req)
    {
        var dto = await _policies.CreateAsync(req, _tenant.TenantId);
        return Created($"/api/v1/policies/{dto.Id}", dto);
    }
}
