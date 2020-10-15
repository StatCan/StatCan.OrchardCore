using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.Hackathon;
using StatCan.OrchardCore.Hackathon.Indexes;
using StatCan.OrchardCore.Hackathon.Services;

namespace StatCan.OrchardCore.Hackathon
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, IndexMigrations>();
            services.AddScoped<IScopedIndexProvider, HackathonItemsIndexProvider>();

            services.AddScoped<IHackathonService, HackathonService>();
            services.AddScoped<IDataMigration, HackathonMigrations>();
        }
    }
}