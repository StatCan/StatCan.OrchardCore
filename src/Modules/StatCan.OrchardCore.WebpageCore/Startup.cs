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
            services.AddScoped<IDataMigration, PageLayoutMigration>();
        }
    }

    [Feature(Constants.Features.ContentLayout)]
    public class ContentLayoutStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ContentLayoutMigration>();
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
