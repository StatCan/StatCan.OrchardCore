using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Widgets
{
    [Feature(Constants.Features.Hero)]
    public class HeroStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, HeroMigration>();
        }
    }

    [Feature(Constants.Features.Page)]
    public class PageStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, PageMigration>();
        }
    }

    [Feature(Constants.Features.Section)]
    public class SectionStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, SectionMigration>();
        }
    }
}
