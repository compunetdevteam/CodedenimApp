using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodedenimWebApp.Startup))]
namespace CodedenimWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
