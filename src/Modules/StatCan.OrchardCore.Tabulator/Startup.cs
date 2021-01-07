using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ResourceManagement;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Tabulator
{
    [Feature(FeatureIds.Tabulator)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection) => serviceCollection.AddScoped<IDataMigration, Migrations>();
    }
}
