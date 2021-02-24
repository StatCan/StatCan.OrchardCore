using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Deployment;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Recipes;
using OrchardCore.Security.Permissions;
using StatCan.OrchardCore.EmailTemplates.Controllers;
using StatCan.OrchardCore.EmailTemplates.Deployment;
using StatCan.OrchardCore.EmailTemplates.Drivers;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Recipes;
using StatCan.OrchardCore.EmailTemplates.Services;

namespace StatCan.OrchardCore.EmailTemplates
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();

            services.AddContentPart<EmailTemplatePart>().UseDisplayDriver<EmailTemplatePartDisplayDriver>();
            services.AddScoped<IContentTypePartDefinitionDisplayDriver, EmailTemplatePartSettingsDisplayDriver>();
            services.AddScoped<EmailTemplatesManager>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddRecipeExecutionStep<EmailTemplateStep>();

            services.AddTransient<IDeploymentSource, AllEmailTemplatesDeploymentSource>();
            services.AddSingleton<IDeploymentStepFactory>(new DeploymentStepFactory<AllEmailTemplatesDeploymentStep>());
            services.AddScoped<IDisplayDriver<DeploymentStep>, AllTemplatesDeploymentStepDriver>();

        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var emailTemplateControllerName = typeof(EmailTemplateController).ControllerName();

            routes.MapAreaControllerRoute(
                name: "EmailTemplates.Index",
                areaName: "StatCan.OrchardCore.EmailTemplates",
                pattern: _adminOptions.AdminUrlPrefix + "/EmailTemplates",
                defaults: new { controller = emailTemplateControllerName, action = nameof(EmailTemplateController.Index) }
            );

            routes.MapAreaControllerRoute(
                name: "EmailTemplates.Create",
                areaName: "StatCan.OrchardCore.EmailTemplates",
                pattern: _adminOptions.AdminUrlPrefix + "/EmailTemplates/Create",
                defaults: new { controller = emailTemplateControllerName, action = nameof(EmailTemplateController.Create) }
            );

            routes.MapAreaControllerRoute(
                name: "EmailTemplates.Edit",
                areaName: "StatCan.OrchardCore.EmailTemplates",
                pattern: _adminOptions.AdminUrlPrefix + "/EmailTemplates/Edit/{name}",
                defaults: new { controller = emailTemplateControllerName, action = nameof(EmailTemplateController.Edit) }
            );
        }
    }
}
