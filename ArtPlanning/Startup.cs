using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArtPlanning.Startup))]
namespace ArtPlanning
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
