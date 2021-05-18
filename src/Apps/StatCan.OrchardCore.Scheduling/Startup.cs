using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.Scheduling.Indexing;
using StatCan.OrchardCore.Scheduling.Models;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Scheduling
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<Appointment>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IDataMigration, IndexMigrations>();
            services.AddSingleton<IIndexProvider, AppointmentIndexProvider>();
            services.AddSingleton<IIndexProvider, AppointmentLinkedIndexProvider>();
        }
    }
}
