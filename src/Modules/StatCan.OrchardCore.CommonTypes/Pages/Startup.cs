using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.CommonTypes.Page
{
    [Feature(FeatureIds.Page)]
    public class PageStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddScoped<IDataMigration, PageMigrations>();
    }

    [Feature(FeatureIds.AdditionalPages)]
    public class AdditionalPagesStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddScoped<IDataMigration, AdditionalPagesMigrations>();
    }
}
