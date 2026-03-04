using InsureX.Application.DTOs;
using InsureX.Application.Services;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using InsureX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsureX.Api.Controllers;

[ApiController]
[Route("api/v1/assets")]
public class AssetsController : ControllerBase
{
    private readonly AssetService _assets;
    private readonly ITenantContext _tenant;

    public AssetsController(AssetService assets, ITenantContext tenant)
    {
        _assets = assets;
        _tenant = tenant;
    }

    /// <summary>GET /api/v1/assets?page=1&amp;pageSize=25&amp;vin=&amp;status=</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<AssetDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? vin = null,
        [FromQuery] string? status = null)
    {
        AssetStatus? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<AssetStatus>(status, true, out var s))
            statusFilter = s;

        var req = new PageRequest { Page = page, PageSize = pageSize };
        return Ok(await _assets.GetAssetsAsync(req, vin, statusFilter));
    }

    /// <summary>GET /api/v1/assets/{id}</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssetDto>> GetById(Guid id)
    {
        var dto = await _assets.GetByIdAsync(id);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    /// <summary>POST /api/v1/assets</summary>
    [HttpPost]
    public async Task<ActionResult<AssetDto>> Create([FromBody] CreateAssetRequest req)
    {
        var dto = await _assets.CreateAsync(req, _tenant.TenantId);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }
}
