using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof( GsEPWv8_5_MVC.Startup))]
namespace  GsEPWv8_5_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
