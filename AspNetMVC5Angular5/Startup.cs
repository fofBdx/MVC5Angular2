using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AspNetMVC5Angular5.Startup))]
namespace AspNetMVC5Angular5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
