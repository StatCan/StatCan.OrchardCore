using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using System;

namespace StatCan.OrchardCore.Hackathon
{
    [Feature(FeatureIds.Team)]
    public class TeamStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, TeamMigrations>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
               name: "JoinTeam",
               areaName: "StatCan.OrchardCore.Hackathon",
               pattern: "team/join",
               defaults: new { controller = "Team", action = "JoinTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "LeaveTeam",
               areaName: "StatCan.OrchardCore.Hackathon",
               pattern: "team/leave",
               defaults: new { controller = "Team", action = "LeaveTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "CreateTeam",
               areaName: "StatCan.OrchardCore.Hackathon",
               pattern: "team/create",
               defaults: new { controller = "Team", action = "CreateTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "RemoveTeamMember",
               areaName: "StatCan.OrchardCore.Hackathon",
               pattern: "team/remove",
               defaults: new { controller = "Team", action = "RemoveTeamMember" }
            );
            routes.MapAreaControllerRoute(
               name: "SaveTeam",
               areaName: "StatCan.OrchardCore.Hackathon",
               pattern: "team/save",
               defaults: new { controller = "Team", action = "SaveTeam" }
            );
        }
    }
}
