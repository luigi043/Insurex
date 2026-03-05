using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.Data.SqlClient;
using IAPR_Data.Classes.Webhook;
using Microsoft.Extensions.Configuration;

namespace IAPR_Data.Classes
{
    // Profile data for the user
    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LegacyUserId { get; set; }
        
        public string? vcName { get; set; }
        public string? vcSurname { get; set; }
        public int iUser_Type_Id { get; set; }
        public int iUser_Status_Id { get; set; }
        public string? vcUser_Status_Description { get; set; }
        public int? iPartner_Type_Id { get; set; }
        public int? iPartner_Id { get; set; }
        public string? vcPosition_Title { get; set; }
        public bool bUserReceiveNotifications { get; set; }

        // Multi-Tenant FKs
        public int? TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant? Tenant { get; set; }

        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization? Organization { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<WebhookEvent> WebhookEvents { get; set; }
        public DbSet<PartnerWebhookConfig> PartnerWebhookConfigs { get; set; }
        public DbSet<ComplianceState> ComplianceStates { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<AuditLogEntry> AuditLog { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseNote> CaseNotes { get; set; }
        public DbSet<ApiClientCredential> ApiClients { get; set; }
        public DbSet<IssuedToken> IssuedTokens { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<IAPR_Data.Classes.Policy.Policy> Policies { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ── Policy entity ─────────────────────────────────────────────────
            // Policy.cs now has [NotMapped] on the navigation properties (primary fix).
            // These fluent API calls are kept as a belt-and-suspenders backup.
            builder.Entity<IAPR_Data.Classes.Policy.Policy>(entity =>
            {
                entity.HasKey(p => p.iPolicy_Id);
                entity.Ignore(p => p.policy_Holder_Individual);
                entity.Ignore(p => p.policy_Holder_Business);
            });

            // ── DTO types – explicitly ignored so EF never maps them ──────────
            // All of these reference Phycisal_address / Postal_Address which are
            // pure DTO value objects with no primary key.
            builder.Ignore<IAPR_Data.Classes.AssetTypes.Vehicle_Asset>();
            builder.Ignore<IAPR_Data.Classes.AssetTypes.API_Vehicle_Asset>();
            builder.Ignore<IAPR_Data.Classes.AssetTypes.Property_Asset>();
            builder.Ignore<IAPR_Data.Classes.Policy.Policy_Holder_Consumer>();
            builder.Ignore<IAPR_Data.Classes.Policy.Policy_Holder_Business>();
        }

        /// <summary>
        /// Returns a LINQ query scoped to the current user's Tenant.
        /// </summary>
        public IQueryable<TEntity> ForTenant<TEntity>(int? tenantId = null)
            where TEntity : class
        {
            var dbSet = Set<TEntity>();

            if (tenantId == null)
                return dbSet;

            var tenantProp = typeof(TEntity).GetProperty("TenantId");
            if (tenantProp == null)
                return dbSet;

            return dbSet.Where(e => EF.Property<int?>(e, "TenantId") == tenantId);
        }

        /// <summary>
        /// Activates SQL Server Row-Level Security for this DB connection.
        /// </summary>
        public void SetRlsTenantContext(int tenantId)
        {
            Database.ExecuteSqlRaw(
                "EXEC dbo.sp_SetTenantContext @TenantId",
                new SqlParameter("@TenantId", tenantId));
        }

        public static ApplicationDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}