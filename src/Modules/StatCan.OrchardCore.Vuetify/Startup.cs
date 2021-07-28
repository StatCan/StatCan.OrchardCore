using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.Vuetify.Drivers;
using StatCan.OrchardCore.Vuetify.Handlers;
using StatCan.OrchardCore.Vuetify.Models;
using StatCan.OrchardCore.Vuetify.Migrations;

namespace StatCan.OrchardCore.Vuetify
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, AlertMigrations>();
            services.AddScoped<IDataMigration, CardMigrations>();
            services.AddScoped<IDataMigration, ExpansionPanelMigrations>();
            services.AddScoped<IDataMigration, GridMigrations>();
            services.AddScoped<IDataMigration, ImageMigrations>();
            services.AddScoped<IDataMigration, ListMigrations>();
            services.AddScoped<IDataMigration, ScheduleMigrations>();
            services.AddScoped<IDataMigration, TabsMigrations>();
            services.AddScoped<IDataMigration, TimelineMigrations>();
           // services.AddScoped<IDataMigration, VuetifyMigrations>();

            services.AddScoped<IContentDisplayDriver, WidgetStylingPartDisplay>();
            services.AddScoped<IContentHandler, WidgetStylingPartHandler>();
            services.AddContentPart<WidgetStylingPart>();


        }
    }
    [Feature(Constants.Features.Alert)]
    public class AlertStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, AlertMigrations>();
        }
    }
    [Feature(Constants.Features.Card)]
    public class CardStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, CardMigrations>();
        }
    }
    [Feature(Constants.Features.ExpansionPanel)]
    public class ExpansionPanelStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ExpansionPanelMigrations>();
        }
    }
    [Feature(Constants.Features.Grid)]
    public class GridStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, GridMigrations>();
        }
    }
    [Feature(Constants.Features.Image)]
    public class ImageStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ImageMigrations>();
        }
    }
    [Feature(Constants.Features.List)]
    public class ListStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ListMigrations>();
        }
    }
    [Feature(Constants.Features.Schedule)]
    public class ScheduleStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ScheduleMigrations>();
        }
    }
    [Feature(Constants.Features.Tabs)]
    public class Tabstartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, TabsMigrations>();
        }
    }
    [Feature(Constants.Features.Timeline)]
    public class TimelineStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, TimelineMigrations>();
        }
    }
}
