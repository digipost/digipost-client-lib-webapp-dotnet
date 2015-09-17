using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DigipostClientLibWebapp.Startup))]
namespace DigipostClientLibWebapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
