using InsureX.Application.Interfaces;
using InsureX.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InsureX.Infrastructure.Data;

public class InsureXDbContext : DbContext, IInsureXDbContext
{
    private readonly Guid _currentTenantId;

    public InsureXDbContext(DbContextOptions<InsureXDbContext> options, ITenantContext tenantContext)
        : base(options)
    {
        _currentTenantId = tenantContext.TenantId;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Organisation> Organisations => Set<Organisation>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Asset> Assets => Set<Asset>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<PolicyEvent> PolicyEvents => Set<PolicyEvent>();
    public DbSet<ComplianceState> ComplianceStates => Set<ComplianceState>();
    public DbSet<NonComplianceCase> Cases => Set<NonComplianceCase>();
    public DbSet<CaseTask> CaseTasks => Set<CaseTask>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Global query filters (tenant isolation) ──────────────────────────
        modelBuilder.Entity<Organisation>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<AppUser>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<Asset>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<Policy>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<PolicyEvent>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<ComplianceState>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<NonComplianceCase>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<CaseTask>().HasQueryFilter(e => e.TenantId == _currentTenantId);
        modelBuilder.Entity<AuditEntry>().HasQueryFilter(e => e.TenantId == _currentTenantId);

        // ── Tenant ────────────────────────────────────────────────────────────
        modelBuilder.Entity<Tenant>(b =>
        {
            b.HasKey(t => t.Id);
            b.Property(t => t.Name).HasMaxLength(200).IsRequired();
            b.Property(t => t.Slug).HasMaxLength(100).IsRequired();
            b.HasIndex(t => t.Slug).IsUnique();
        });

        // ── Organisation ─────────────────────────────────────────────────────
        modelBuilder.Entity<Organisation>(b =>
        {
            b.HasKey(o => o.Id);
            b.Property(o => o.Name).HasMaxLength(200).IsRequired();
            b.HasIndex(o => new { o.TenantId, o.Name });
            b.HasOne(o => o.Tenant).WithMany(t => t.Organisations).HasForeignKey(o => o.TenantId);
        });

        // ── AppUser ───────────────────────────────────────────────────────────
        modelBuilder.Entity<AppUser>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.Email).HasMaxLength(256).IsRequired();
            b.HasIndex(u => new { u.TenantId, u.Email }).IsUnique();
            b.HasOne(u => u.Tenant).WithMany(t => t.Users).HasForeignKey(u => u.TenantId);
            b.HasOne(u => u.Organisation).WithMany(o => o.Users).HasForeignKey(u => u.OrgId);
        });

        // ── Asset ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Asset>(b =>
        {
            b.HasKey(a => a.Id);
            b.Property(a => a.VIN).HasMaxLength(50);
            b.Property(a => a.RegistrationNumber).HasMaxLength(50);
            b.HasIndex(a => new { a.TenantId, a.VIN });
            b.HasIndex(a => new { a.TenantId, a.Status });
            b.HasOne(a => a.Organisation).WithMany(o => o.Assets).HasForeignKey(a => a.OrgId);
            b.HasOne(a => a.ComplianceState).WithOne(c => c.Asset).HasForeignKey<ComplianceState>(c => c.AssetId);
        });

        // ── Policy ────────────────────────────────────────────────────────────
        modelBuilder.Entity<Policy>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.PolicyNumber).HasMaxLength(100).IsRequired();
            b.HasIndex(p => new { p.TenantId, p.PolicyNumber });
            b.HasOne(p => p.Asset).WithMany(a => a.Policies).HasForeignKey(p => p.AssetId);
            b.HasOne(p => p.InsurerOrg).WithMany().HasForeignKey(p => p.InsurerOrgId);
        });

        // ── PolicyEvent ───────────────────────────────────────────────────────
        modelBuilder.Entity<PolicyEvent>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.SourceEventId).HasMaxLength(200);
            b.HasIndex(e => new { e.TenantId, e.SourceSystem, e.SourceEventId }).IsUnique();
            b.HasOne(e => e.Policy).WithMany(p => p.Events).HasForeignKey(e => e.PolicyId);
        });

        // ── ComplianceState ───────────────────────────────────────────────────
        modelBuilder.Entity<ComplianceState>(b =>
        {
            b.HasKey(c => c.Id);
            b.HasIndex(c => new { c.TenantId, c.Status });
        });

        // ── NonComplianceCase ─────────────────────────────────────────────────
        modelBuilder.Entity<NonComplianceCase>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.CaseNumber).HasMaxLength(50).IsRequired();
            b.HasIndex(c => new { c.TenantId, c.CaseNumber }).IsUnique();
            b.HasIndex(c => new { c.TenantId, c.Status });
            b.HasOne(c => c.Asset).WithMany(a => a.Cases).HasForeignKey(c => c.AssetId);
        });

        // ── CaseTask ──────────────────────────────────────────────────────────
        modelBuilder.Entity<CaseTask>(b =>
        {
            b.HasKey(t => t.Id);
            b.HasOne(t => t.Case).WithMany(c => c.Tasks).HasForeignKey(t => t.CaseId);
        });

        // ── AuditEntry (append-only) ──────────────────────────────────────────
        modelBuilder.Entity<AuditEntry>(b =>
        {
            b.HasKey(a => a.Id);
            b.HasIndex(a => new { a.TenantId, a.EntityType, a.EntityId });
            b.HasIndex(a => a.CreatedUtc);
            b.HasOne(a => a.Case).WithMany(c => c.AuditEntries).HasForeignKey(a => a.CaseId).IsRequired(false);
        });
    }
}
