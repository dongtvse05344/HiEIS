using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HiEIS.Startup))]
namespace HiEIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
