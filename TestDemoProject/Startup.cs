using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InterviewSystem.Startup))]
namespace InterviewSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
