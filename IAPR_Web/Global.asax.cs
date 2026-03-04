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

namespace IAPR_Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Seed Default Admin User & generate EF Database
            using (var dbContext = new ApplicationDbContext())
            {
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
                        };
                        userManager.Create(seedUser, "Password123!");
                    }
                }
            }
        }
    }
}