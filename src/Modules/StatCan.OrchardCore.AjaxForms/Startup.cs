using System;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
using OrchardCore.Data.Migration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ContentManagement;
using StatCan.OrchardCore.AjaxForms.Models;
using OrchardCore.Navigation;
using System.Linq;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.AjaxForms.Workflows;

namespace StatCan.OrchardCore.AjaxForms
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<AjaxForm>();
            services.AddContentPart<AjaxFormScripts>();
            services.AddContentPart<FormInput>();
            services.AddContentPart<FormInputStyle>();
            services.AddContentPart<FormRequiredValidation>();
            services.AddContentPart<FormButton>();

            services.AddScoped<IDataMigration, Migrations>();
        }
         public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "AjaxFormSubmit",
                areaName: "StatCan.OrchardCore.AjaxForms",
                pattern: "ajaxforms/submit/{formId}",
                defaults: new { controller = "AjaxForm", action = "Submit" }
            );
        }
    }

    [RequireFeatures("OrchardCore.AdminMenu")]
    public class MenuStartup: StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<INavigationProvider, AdminMenu>();
        }
    }
    [RequireFeatures("OrchardCore.Workflows")]
    public class WorkflowsStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddActivity<AjaxFormSubmittedEvent, AjaxFormSubmittedEventDisplayDriver>();
        }
    }
}
