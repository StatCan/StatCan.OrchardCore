using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;

namespace StatCan.Themes.HackathonTheme
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IResourceManifestProvider, ResourceManifest>();
        }
    }
}