using InsureX.Application.DTOs;
using InsureX.Application.Services;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using InsureX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsureX.Api.Controllers;

[ApiController]
[Route("api/v1/compliance")]
public class ComplianceController : ControllerBase
{
    private readonly ComplianceService _compliance;
    private readonly ITenantContext _tenant;

    public ComplianceController(ComplianceService compliance, ITenantContext tenant)
    {
        _compliance = compliance;
        _tenant = tenant;
    }

    /// <summary>GET /api/v1/compliance/assets?status=NonCompliant&amp;page=1</summary>
    [HttpGet("assets")]
    public async Task<ActionResult<PagedResult<ComplianceStateDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? status = null)
    {
        ComplianceStatus? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ComplianceStatus>(status, true, out var s))
            statusFilter = s;

        return Ok(await _compliance.GetComplianceListAsync(new PageRequest { Page = page, PageSize = pageSize }, statusFilter));
    }

    /// <summary>POST /api/v1/compliance/evaluate/{assetId} — re-evaluate on demand</summary>
    [HttpPost("evaluate/{assetId:guid}")]
    public async Task<IActionResult> Evaluate(Guid assetId)
    {
        await _compliance.EvaluateAsync(assetId, _tenant.TenantId);
        return NoContent();
    }
}
