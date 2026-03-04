using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

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
        public ApplicationDbContext()
            : base("connIAPRData")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
