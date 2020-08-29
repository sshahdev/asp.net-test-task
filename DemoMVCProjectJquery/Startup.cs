using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DemoMVCProjectJquery.Startup))]
namespace DemoMVCProjectJquery
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
