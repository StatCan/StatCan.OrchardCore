using Lombiq.HelpfulExtensions;
using Lombiq.HelpfulExtensions.Extensions.Flows;
using Lombiq.HelpfulExtensions.Extensions.Flows.Drivers;
using Lombiq.HelpfulExtensions.Extensions.Flows.Handlers;
using Lombiq.HelpfulExtensions.Extensions.Flows.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.DisplayManagement.Descriptors;
using OrchardCore.Modules;
using System;

namespace Piedone.HelpfulExtensions.Extensions.Flows
{
    [Feature(FeatureIds.Flows)]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IContentDisplayDriver, AdditionalStylingPartDisplay>();
            services.AddScoped<IContentHandler, AdditionalStylingPartHandler>();
            services.AddContentPart<AdditionalStylingPart>();
            services.AddScoped<IShapeTableProvider, FlowPartShapeTableProvider>();
        }

        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // No need for anything here yet.
        }
    }
}
