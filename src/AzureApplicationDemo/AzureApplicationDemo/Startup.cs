using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureApplicationDemo.Startup))]
namespace AzureApplicationDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ContainerConfig.RegisterServices(app);
            ConfigureAuth(app);
        }
    }
}
