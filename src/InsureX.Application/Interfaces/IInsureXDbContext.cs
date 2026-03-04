using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Application.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext, defined in Application so services
/// have no direct dependency on Infrastructure. Infrastructure implements this.
/// </summary>
public interface IInsureXDbContext
{
    DbSet<Tenant> Tenants { get; }
    DbSet<Organisation> Organisations { get; }
    DbSet<AppUser> Users { get; }
    DbSet<Asset> Assets { get; }
    DbSet<Policy> Policies { get; }
    DbSet<PolicyEvent> PolicyEvents { get; }
    DbSet<ComplianceState> ComplianceStates { get; }
    DbSet<NonComplianceCase> Cases { get; }
    DbSet<CaseTask> CaseTasks { get; }
    DbSet<AuditEntry> AuditEntries { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
