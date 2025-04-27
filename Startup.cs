using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Laba3.Startup))]
namespace Laba3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
