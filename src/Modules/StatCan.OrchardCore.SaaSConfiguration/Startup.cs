using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using StatCan.OrchardCore.SaaSConfiguration.Controllers;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    [Feature(Constants.Features.SaaSConfiguration)]
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
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IDisplayDriver<ISite>, SaaSConfigurationSettingsDisplayDriver>();
            services.AddScoped<ISaaSConfigurationService, SaaSConfigurationService>();

            // Note: the following service are registered using TryAdd to prevent duplicate registrations.
            services.TryAdd(ServiceDescriptor.Scoped<ITenantHelperService, TenantHelperService>());
        }
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var adminControllerName = typeof(AdminController).ControllerName();
            routes.MapAreaControllerRoute(
                name: "ExecuteJsonRoute",
                areaName: Constants.Features.SaaSConfiguration,
                pattern: _adminOptions.AdminUrlPrefix + "/SaaS/ExecuteRecipe",
                defaults: new { controller = adminControllerName, action = "ExecuteRecipe" }
            );
        }
    }

    [Feature(Constants.Features.SaaSConfigurationClient)]
    public class ClientStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, ClientMigrations>();

            // Note: the following service are registered using TryAdd to prevent duplicate registrations.
            services.TryAdd(ServiceDescriptor.Scoped<ITenantHelperService, TenantHelperService>());
        }
    }
}
