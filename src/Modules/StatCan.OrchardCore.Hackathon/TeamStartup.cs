using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Hackathon
{
    [RequireFeatures("StatCan.Orchardcore.Hackathon.Team")]
    public class TeamStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, TeamMigrations>();
        }
    }
}
