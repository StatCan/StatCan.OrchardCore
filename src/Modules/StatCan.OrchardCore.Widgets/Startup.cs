using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Widgets
{
    [Feature(Constants.Features.PageLayout)]
    public class PageLayoutStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, FatFooterMigration>();
            services.AddScoped<IDataMigration, HeroMigration>();
            services.AddScoped<IDataMigration, PageMigration>();
            services.AddScoped<IDataMigration, SectionMigration>();
        }
    }
}
