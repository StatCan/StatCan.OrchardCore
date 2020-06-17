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

namespace StatCan.OrchardCore.AjaxForms
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().AddMvcOptions(options =>
            //{
            //    if (!options.ModelValidatorProviders.Any(x => x is FormModelValidationProvider))
            //    {
            //        options.ModelValidatorProviders.Insert(0, new FormModelValidationProvider());
            //    }
            //}).AddViewOptions(viewOptions =>
            //{
                
            //    if (!viewOptions.ClientModelValidatorProviders.Any(x => x is FormClientModelValidationProvider))
            //    {
            //        viewOptions.ClientModelValidatorProviders.Insert(0, new FormClientModelValidationProvider());
            //    }
            //});

            services.AddContentPart<AjaxForm>();
            services.AddContentPart<FormInput>();
            services.AddContentPart<FormInputStyle>();
            services.AddContentPart<FormRequiredValidation>();
            services.AddContentPart<FormButton>();

            services.AddScoped<INavigationProvider, AdminMenu>();
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
}
