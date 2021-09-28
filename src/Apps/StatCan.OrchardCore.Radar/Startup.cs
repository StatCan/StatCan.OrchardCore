using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using StatCan.OrchardCore.Radar.Indexing;
using StatCan.OrchardCore.Radar.Models;
using OrchardCore.ResourceManagement;
using Microsoft.Extensions.Options;

namespace StatCan.OrchardCore.Radar
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddContentPart<RadarEntityPart>();
            services.AddSingleton<IIndexProvider, RadarEntityPartIndexProvider>();
            services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Home",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}
