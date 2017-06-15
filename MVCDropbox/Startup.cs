using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCDropbox.Startup))]
namespace MVCDropbox
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
