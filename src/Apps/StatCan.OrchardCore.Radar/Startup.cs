using System;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using YesSql.Indexes;
using OrchardCore.Modules;
using OrchardCore.Data.Migration;
using OrchardCore.ResourceManagement;
using OrchardCore.Liquid;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using StatCan.OrchardCore.Radar.Filters;
using StatCan.OrchardCore.Radar.Migrations;
using StatCan.OrchardCore.Radar.Models;
using StatCan.OrchardCore.Radar.Indexes;
using StatCan.OrchardCore.Radar.Drivers;

namespace StatCan.OrchardCore.Radar
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ContentTypeMigrations>();

            services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(typeof(ResourceInjectionFilter));
            });

            services.Configure<TemplateOptions>(options =>
            {
                options.MemberAccessStrategy.Register<RadarFormPart>();
            });

            services.AddContentPart<RadarFormPart>()
                        .UseDisplayDriver<RadarFormPartDisplayDriver>();
            services.AddSingleton<IIndexProvider, RadarFormPartIndexProvider>();
            services.AddScoped<IDataMigration, IndexMigrations>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // List view routes
            routes.MapAreaControllerRoute(
                name: "ProjectListView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "projects",
                defaults: new { controller = "List", action = "List" },
                dataTokens: new { type = "Project" }
            );

            routes.MapAreaControllerRoute(
                name: "ProposalListView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "proposals",
                defaults: new { controller = "List", action = "List" },
                dataTokens: new { type = "Proposal" }
            );

            routes.MapAreaControllerRoute(
                name: "EventListView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "events",
                defaults: new { controller = "List", action = "List" },
                dataTokens: new { type = "Event" }
            );

            routes.MapAreaControllerRoute(
                name: "CommunityListView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "communities",
                defaults: new { controller = "List", action = "List" },
                dataTokens: new { type = "Community" }
            );

            // Form view routes
            routes.MapAreaControllerRoute(
                name: "TopicFormCreateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "topics/create",
                defaults: new { controller = "Form", action = "TopicForm" }
            );

            routes.MapAreaControllerRoute(
                name: "TopicFormUpdateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "topics/update/{id}",
                defaults: new { controller = "Form", action = "TopicForm" }
            );

            // search api endpoints
            routes.MapAreaControllerRoute(
                name: "ListSearchAPI",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "api/radar/list-search",
                defaults: new { controller = "List", action = "Search" }
            );

            // search api endpoints
            routes.MapAreaControllerRoute(
                name: "GlobalSearchAPI",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "api/radar/global-search",
                defaults: new { controller = "List", action = "GlobalSearch" }
            );
        }
    }
}
