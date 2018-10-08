using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WeddingPlannerProject.Startup))]
namespace WeddingPlannerProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
