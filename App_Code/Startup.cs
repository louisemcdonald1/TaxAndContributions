using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaxAndContributions.Startup))]
namespace TaxAndContributions
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
