using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;
using OrchardCore.ContentManagement;
using YesSql.Indexes;
using OrchardCore.ResourceManagement;
using OrchardCore.Liquid;
using StatCan.OrchardCore.Radar.Indexing;
using StatCan.OrchardCore.Radar.Models;
using StatCan.OrchardCore.Radar.Liquid;
using StatCan.OrchardCore.Radar.Filters;

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

            services.AddLiquidFilter<ListCanViewFilter>("list_can_view");
            services.AddLiquidFilter<CurrentCultureFilter>("current_culture");

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ResourceInjectionFilter));
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
