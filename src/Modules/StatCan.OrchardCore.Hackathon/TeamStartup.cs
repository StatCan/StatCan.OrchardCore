using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using System;

namespace StatCan.OrchardCore.Hackathon
{
    [RequireFeatures("StatCan.Orchardcore.Hackathon.Team")]
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
               areaName: "StatCan.Orchardcore.Hackathon",
               pattern: "team/join",
               defaults: new { controller = "Dashboard", action = "JoinTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "LeaveTeam",
               areaName: "StatCan.Orchardcore.Hackathon",
               pattern: "team/leave",
               defaults: new { controller = "Dashboard", action = "LeaveTeam" }
            );
            routes.MapAreaControllerRoute(
               name: "CreateTeam",
               areaName: "StatCan.Orchardcore.Hackathon",
               pattern: "team/create",
               defaults: new { controller = "Dashboard", action = "CreateTeam" }
            );
        }
    }
}
