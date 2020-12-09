using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.Hackathon.Indexes;
using StatCan.OrchardCore.Hackathon.Services;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Hackathon
{
    [Feature(FeatureIds.Hackathon)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, IndexMigrations>();
            services.AddScoped<IScopedIndexProvider, HackathonItemsIndexProvider>();
            services.AddSingleton<IIndexProvider, HackathonUsersIndexProvider>();

            services.AddScoped<IHackathonService, HackathonService>();
            services.AddScoped<IDataMigration, HackathonMigrations>();
        }
    }
}
