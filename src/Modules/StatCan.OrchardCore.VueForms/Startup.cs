using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.VueForms.Models;
using StatCan.OrchardCore.VueForms.Workflows;

namespace StatCan.OrchardCore.VueForms
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<VueForm>();
            services.AddContentPart<VueFormScripts>();

            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
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
