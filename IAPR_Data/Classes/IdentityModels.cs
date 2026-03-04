using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.SqlClient;
using IAPR_Data.Classes.Webhook;

namespace IAPR_Data.Classes
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class.
    public class ApplicationUser : IdentityUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LegacyUserId { get; set; }
        // Add custom properties to match the legacy CurrentUser
        public string vcName { get; set; }
        public string vcSurname { get; set; }
        public int iUser_Type_Id { get; set; }
        public int iUser_Status_Id { get; set; }
        public string vcUser_Status_Description { get; set; }
        public int? iPartner_Type_Id { get; set; }
        public int? iPartner_Id { get; set; }
        public string vcPosition_Title { get; set; }
        public bool bUserReceiveNotifications { get; set; }

        // Multi-Tenant FKs
        public int? TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        public int? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        
        // These can be populated securely or managed by Identity
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<WebhookEvent> WebhookEvents { get; set; }
        public DbSet<ComplianceState> ComplianceStates { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<AuditLogEntry> AuditLog { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CaseNote> CaseNotes { get; set; }

        public ApplicationDbContext()
            : base("connIAPRData")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        /// <summary>
        /// Returns a LINQ query scoped to the current user's Tenant.
        /// Uses TenantContext to resolve the active TenantId from OWIN claims.
        /// </summary>
        /// <param name="tenantId">Optional override; if null, uses TenantContext.Current</param>
        public IQueryable<TEntity> ForTenant<TEntity>(int? tenantId = null)
            where TEntity : class
        {
            int? resolvedTenantId = tenantId ?? TenantContext.Current;
            var dbSet = Set<TEntity>();

            if (resolvedTenantId == null)
                return dbSet; // No tenant restriction for system-level queries

            // Apply TenantId filter via reflection-based convention
            var tenantProp = typeof(TEntity).GetProperty("TenantId");
            if (tenantProp == null)
                return dbSet; // Entity does not participate in multi-tenancy

            return dbSet.Where(e => (int?)tenantProp.GetValue(e, null) == resolvedTenantId);
        }

        /// <summary>
        /// Activates SQL Server Row-Level Security for this DB connection
        /// by calling sp_SetTenantContext with the current user's TenantId.
        /// Must be called once per connection, immediately after opening.
        /// </summary>
        public void SetRlsTenantContext(int tenantId)
        {
            Database.ExecuteSqlCommand(
                "EXEC dbo.sp_SetTenantContext @TenantId",
                new SqlParameter("@TenantId", tenantId));
        }
    }
}
