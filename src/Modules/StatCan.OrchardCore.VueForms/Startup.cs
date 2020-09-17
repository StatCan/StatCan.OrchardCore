using System;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using OrchardCore.Data.Migration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ContentManagement;
using StatCan.OrchardCore.VueForms.Models;
using OrchardCore.Navigation;
using System.Linq;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.VueForms.Workflows;
using OrchardCore.ResourceManagement;

namespace StatCan.OrchardCore.VueForms
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<VueForm>();
            services.AddContentPart<VueFormScripts>();
            services.AddContentPart<VueComponent>();

            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddScoped<IResourceManifestProvider, ResourceManifest>();
        }
         public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "VueFormSubmit",
                areaName: "StatCan.OrchardCore.VueForms",
                pattern: "vueforms/submit/{formId}",
                defaults: new { controller = "VueForm", action = "Submit" }
            );
        }
    }

    [RequireFeatures("OrchardCore.Workflows")]
    public class WorkflowsStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<VueFormSubmittedEvent, VueFormSubmittedEventDisplayDriver>();
        }
    }

    [Feature(Constants.Features.Localized)]
    public class LocalizedStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, LocalizationMigrations>();
        }
    }
}
