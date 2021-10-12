using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.Candev.Indexes;
using StatCan.OrchardCore.Candev.Services;
using System;
using YesSql.Indexes;

namespace StatCan.OrchardCore.Candev
{
    [Feature(FeatureIds.Candev)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, IndexMigrations>();
            services.AddScoped<IScopedIndexProvider, HackathonItemsIndexProvider>();
            services.AddSingleton<IIndexProvider, HackathonUsersIndexProvider>();

            services.AddScoped<ICandevService, CandevService>();
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IContentDisplayDriver, CandevDriver>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
               name: "JoinTeam",
               areaName: "StatCan.OrchardCore.Candev",
               pattern: "team/join",
               defaults: new { controller = "Candev", action = "JoinTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "LeaveTeam",
               areaName: "StatCan.OrchardCore.Candev",
               pattern: "team/leave",
               defaults: new { controller = "Candev", action = "LeaveTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "CreateTeam",
               areaName: "StatCan.OrchardCore.Candev",
               pattern: "team/create",
               defaults: new { controller = "Candev", action = "CreateTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "RemoveTeamMember",
               areaName: "StatCan.OrchardCore.Candev",
               pattern: "team/remove",
               defaults: new { controller = "Candev", action = "RemoveTeamMember" }
            );
            routes.MapAreaControllerRoute(
               name: "SaveTeam",
               areaName: "StatCan.OrchardCore.Candev",
               pattern: "team/save",
               defaults: new { controller = "Candev", action = "SaveTeam" }
            );
        }
    }
}
