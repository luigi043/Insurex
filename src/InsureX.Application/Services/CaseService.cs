using InsureX.Application.DTOs;
using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Application.Services;

public class CaseService
{
    private readonly IInsureXDbContext _db;

    public CaseService(IInsureXDbContext db)
    {
        _db = db;
    }

    public async Task<PagedResult<CaseDto>> GetCasesAsync(PageRequest req, CaseStatus? statusFilter = null)
    {
        var query = _db.Cases.Include(c => c.Asset).AsQueryable();

        if (statusFilter.HasValue)
            query = query.Where(c => c.Status == statusFilter.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.OpenedUtc)
            .Skip((req.ValidPage - 1) * req.ValidPageSize)
            .Take(req.ValidPageSize)
            .Select(c => new CaseDto
            {
                Id = c.Id,
                CaseNumber = c.CaseNumber,
                AssetId = c.AssetId,
                AssetVin = c.Asset.VIN,
                Status = c.Status.ToString(),
                ReasonCode = c.ReasonCode,
                OpenedUtc = c.OpenedUtc,
                SlaDeadlineUtc = c.SlaDeadlineUtc,
                ResolvedUtc = c.ResolvedUtc
            })
            .ToListAsync();

        return new PagedResult<CaseDto> { Items = items, Page = req.ValidPage, PageSize = req.ValidPageSize, TotalItems = total };
    }

    public async Task<bool> UpdateStatusAsync(Guid caseId, string action, Guid actorUserId, Guid tenantId)
    {
        var c = await _db.Cases.FindAsync(caseId);
        if (c == null) return false;

        var prevStatus = c.Status;

        c.Status = action.ToLower() switch
        {
            "escalate" => CaseStatus.Escalated,
            "close"    => CaseStatus.Closed,
            "resolve"  => CaseStatus.Resolved,
            "assign"   => CaseStatus.InProgress,
            _          => c.Status
        };

        if (c.Status == CaseStatus.Resolved) c.ResolvedUtc = DateTime.UtcNow;
        if (c.Status == CaseStatus.Closed)   c.ClosedUtc   = DateTime.UtcNow;

        // Audit
        _db.AuditEntries.Add(new AuditEntry
        {
            TenantId = tenantId,
            UserId = actorUserId,
            EntityType = "NonComplianceCase",
            EntityId = caseId,
            CaseId = caseId,
            Action = action,
            OldValues = prevStatus.ToString(),
            NewValues = c.Status.ToString(),
            CorrelationId = Guid.NewGuid().ToString()
        });

        await _db.SaveChangesAsync();
        return true;
    }
}
