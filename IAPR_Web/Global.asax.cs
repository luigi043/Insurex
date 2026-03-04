using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using IAPR_Data.Classes;
using System.Data.Entity;
using IAPR_Data.Services;

namespace IAPR_Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Start the Compliance Engine — it registers itself with the WebhookEventQueue and starts the queue worker.
            ComplianceEngine.Instance.Start();

            // Start the Outbox pattern publisher — polls the DB for unpublished compliance outcome messages.
            OutboxPublisher.Instance.Start();

            // Start the Case Manager — runs SLA escalation sweep every 5 minutes.
            CaseManager.Instance.Start();

            // Explicitly force database recreation if the model has changed
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());

            // Seed Default Admin User & generate EF Database
            using (var dbContext = new ApplicationDbContext())
            {
                // Force Initialization
                dbContext.Database.Initialize(force: true);

                // 1. Seed System Tenant
                var systemTenant = dbContext.Tenants.FirstOrDefault(t => t.DomainKey == "system");
                if (systemTenant == null)
                {
                    systemTenant = new Tenant
                    {
                        Name = "System Default Tenant",
                        DomainKey = "system"
                    };
                    dbContext.Tenants.Add(systemTenant);
                    dbContext.SaveChanges(); // Need ID for Organization
                }

                // 2. Seed HQ Organization
                var hqOrg = dbContext.Organizations.FirstOrDefault(o => o.TenantId == systemTenant.Id && o.Type == "HQ");
                if (hqOrg == null)
                {
                    hqOrg = new Organization
                    {
                        TenantId = systemTenant.Id,
                        Name = "Headquarters",
                        Type = "HQ"
                    };
                    dbContext.Organizations.Add(hqOrg);
                    dbContext.SaveChanges(); // Need ID for User
                }

                var userStore = new UserStore<ApplicationUser>(dbContext);
                using (var userManager = new UserManager<ApplicationUser>(userStore))
                {
                    if (userManager.FindByName("admin@insurex.com") == null)
                    {
                        var seedUser = new ApplicationUser
                        {
                            UserName = "admin@insurex.com",
                            vcName = "System",
                            vcSurname = "Administrator",
                            iUser_Type_Id = 1, // Admin role
                            iUser_Status_Id = 1, // Active
                            TenantId = systemTenant.Id,
                            OrganizationId = hqOrg.Id
                        };
                        userManager.Create(seedUser, "Password123!");
                    }
                }

                // 3. Apply SQL Server Row-Level Security (RLS) policies
                // Uses idempotent SQL: checks for object existence before creating.
                // Wrapped in try/catch so RLS permission issues don't block startup in dev.
                try
                {
                    var rlsSqlPath = System.IO.Path.Combine(
                        System.AppDomain.CurrentDomain.BaseDirectory,
                        "..\\docs\\sql\\rls_setup.sql");

                    if (System.IO.File.Exists(rlsSqlPath))
                    {
                        var rlsSql = System.IO.File.ReadAllText(rlsSqlPath);
                        // Execute each GO-delimited batch separately
                        var batches = rlsSql.Split(
                            new[] { "\r\nGO\r\n", "\nGO\n", "\r\nGO\n" },
                            System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var batch in batches)
                        {
                            var trimmed = batch.Trim();
                            if (!string.IsNullOrWhiteSpace(trimmed))
                                dbContext.Database.ExecuteSqlCommand(trimmed);
                        }
                    }
                }
                catch (Exception rlsEx)
                {
                    // RLS setup is non-fatal: log and continue
                    System.Diagnostics.Trace.TraceWarning(
                        "RLS setup skipped: " + rlsEx.Message);
                }
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            // Gracefully drain and stop the background workers
            WebhookEventQueue.Instance.Stop();
            OutboxPublisher.Instance.Stop();
            CaseManager.Instance.Stop();
        }
    }
}