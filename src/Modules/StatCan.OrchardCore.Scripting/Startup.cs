using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Scripting
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGlobalMethodProvider, FormsGlobalMethodsProvider>();
        }
    }
    [RequireFeatures("OrchardCore.Contents")]
    public class ContentStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGlobalMethodProvider, ContentGlobalMethodsProvider>();
        }
    }
    [RequireFeatures("OrchardCore.ContentLocalization")]
    public class ContentLocalizationStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGlobalMethodProvider, LocalizationGlobalMethodsProvider>();
        }
    }
}