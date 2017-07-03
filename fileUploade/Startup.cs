using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(fileUploade.Startup))]
namespace fileUploade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
