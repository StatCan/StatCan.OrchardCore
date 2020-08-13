using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.WebpageCore
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

    [Feature(Constants.Features.ContentLayout)]
    public class ContentLayoutStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ShowcaseBlurbMigration>();
        }
    }

    [Feature(Constants.Features.MenuItemParts)]
    public class MenuItemPartsStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, MenuItemPartsMigration>();
        }
    }
}
