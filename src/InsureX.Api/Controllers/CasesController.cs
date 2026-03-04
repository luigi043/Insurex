using InsureX.Application.DTOs;
using InsureX.Application.Services;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using InsureX.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InsureX.Api.Controllers;

[ApiController]
[Route("api/v1/cases")]
public class CasesController : ControllerBase
{
    private readonly CaseService _cases;
    private readonly ITenantContext _tenant;

    public CasesController(CaseService cases, ITenantContext tenant)
    {
        _cases = cases;
        _tenant = tenant;
    }

    /// <summary>GET /api/v1/cases?status=Open&amp;page=1</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<CaseDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25,
        [FromQuery] string? status = null)
    {
        CaseStatus? statusFilter = null;
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<CaseStatus>(status, true, out var s))
            statusFilter = s;

        return Ok(await _cases.GetCasesAsync(new PageRequest { Page = page, PageSize = pageSize }, statusFilter));
    }

    /// <summary>POST /api/v1/cases/{id}/actions/{action}</summary>
    [HttpPost("{id:guid}/actions/{action}")]
    public async Task<IActionResult> Action(Guid id, string action)
    {
        // TODO: replace Guid.Empty with real user from auth claims
        var success = await _cases.UpdateStatusAsync(id, action, Guid.Empty, _tenant.TenantId);
        if (!success) return NotFound();
        return NoContent();
    }
}
