using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialMediasAssistant.Startup))]
namespace SocialMediasAssistant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
