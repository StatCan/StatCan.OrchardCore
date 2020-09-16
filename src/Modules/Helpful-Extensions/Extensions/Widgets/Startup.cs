using Lombiq.HelpfulExtensions;
using Lombiq.HelpfulExtensions.Extensions.Widgets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using System;

namespace Piedone.HelpfulExtensions.Extensions.Widgets
{
    [Feature(FeatureIds.Widgets)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services) => services.AddScoped<IDataMigration, Migrations>();

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // No need for anything here yet.
        }
    }
}
