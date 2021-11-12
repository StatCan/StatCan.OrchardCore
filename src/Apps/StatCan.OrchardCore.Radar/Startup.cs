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
using StatCan.OrchardCore.Radar.Services;

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

            services.AddScoped<FormValueProvider>();
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
                name: "FormCreateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "{entityType:regex(^(topics|projects|events|communities|proposals)$)}/create",
                defaults: new { controller = "Form", action = "Form" }
            );

            routes.MapAreaControllerRoute(
                name: "FormUpdateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "{entityType:regex(^(topics|projects|events|communities|proposals)$)}/update/{id}",
                defaults: new { controller = "Form", action = "Form" }
            );

            // Special Cases
            routes.MapAreaControllerRoute(
                name: "FormContainedCreateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "{parentType:regex(^(projects|events|communities|proposals)$)}/{parentId}/{childType:regex(^(artifacts)$)}/create",
                defaults: new { controller = "Form", action = "FormContained" }
            );

            routes.MapAreaControllerRoute(
                name: "FormContainedUpdateView",
                areaName: "StatCan.OrchardCore.Radar",
                pattern: "{parentType:regex(^(projects|events|communities|proposals)$)}/{parentId}/{childType:regex(^(artifacts)$)}/update/{id}",
                defaults: new { controller = "Form", action = "FormContained" }
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
