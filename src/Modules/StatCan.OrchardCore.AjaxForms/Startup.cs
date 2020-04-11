using System;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OrchardCore.Data.Migration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.ContentManagement;
using StatCan.OrchardCore.AjaxForms.Models;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.AjaxForms
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<INavigationProvider, AdminMenu>();

            services.AddContentPart<AjaxForm>();
            services.AddContentPart<FormTextInput>();
            services.AddContentPart<FormButton>();
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
}
