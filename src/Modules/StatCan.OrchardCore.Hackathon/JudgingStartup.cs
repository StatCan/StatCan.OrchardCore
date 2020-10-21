using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Hackathon
{
    [RequireFeatures("StatCan.OrchardCore.Hackathon.Judging")]
    public class JudgingStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<IPermissionProvider, JudgingPermissions>();
            //services.AddScoped<IJudgingService, JudgingService>();
            services.AddScoped<IDataMigration, JudgingMigrations>();
        }

        /*public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "DisplayJudgingEntry",
                areaName: "Inno.Hackathons",
                pattern: "JudgingEntry/{contentItemId}",
                defaults: new { controller = "Judging", action = "DisplayEntry" }
            );
            routes.MapAreaControllerRoute(
                name: "PublishJudgingEntry",
                areaName: "Inno.Hackathons",
                pattern: "JudgingEntry/publish",
                defaults: new { controller = "Judging", action = "PublishJudgingEntry" }
            );
            routes.MapAreaControllerRoute(
                name: "ConcludeHacking",
                areaName: "Inno.Hackathons",
                pattern: "Admin/Hackathons/dashboard/{hackathonLocalizationSet}/conclude-hacking",
                defaults: new { controller = "JudgingAdmin", Action = "ConcludeHacking" }
            );

            routes.MapAreaControllerRoute(
                name: "JudgingRoundPage",
                areaName: "Inno.Hackathons",
                pattern: "Admin/Hackathons/dashboard/{hackathonLocalizationSet}/judging-round",
                defaults: new { controller = "JudgingAdmin", Action = "JudgingRound" }
            );

            routes.MapAreaControllerRoute(
                name: "EliminateTeam",
                areaName: "Inno.Hackathons",
                pattern: "Admin/Hackathons/dashboard/{hackathonLocalizationSet}/eliminate-team",
                defaults: new { controller = "JudgingAdmin", Action = "EliminateTeam" }
            );

            routes.MapAreaControllerRoute(
                name: "StartJudgingRound",
                areaName: "Inno.Hackathons",
                pattern: "Admin/Hackathons/dashboard/{hackathonLocalizationSet}/start-round",
                defaults: new { controller = "JudgingAdmin", Action = "StartJudgingRound" }
            );

            routes.MapAreaControllerRoute(
                name: "EndJudgingRound",
                areaName: "Inno.Hackathons",
                pattern: "Admin/Hackathons/dashboard/{hackathonLocalizationSet}/end-round",
                defaults: new { controller = "JudgingAdmin", Action = "EndJudgingRound" }
            );
        }*/
    }
}