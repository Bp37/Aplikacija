using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SigurnostAplikacija.Startup))]
namespace SigurnostAplikacija
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
