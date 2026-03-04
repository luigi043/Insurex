using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IAPR_Web.Startup))]
namespace IAPR_Web
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
