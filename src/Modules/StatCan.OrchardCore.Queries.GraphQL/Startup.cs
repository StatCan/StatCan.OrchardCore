using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using StatCan.OrchardCore.Queries.GraphQL.Drivers;
using OrchardCore.Security.Permissions;
using OrchardCore.Queries;
using Microsoft.AspNetCore.Builder;
using StatCan.OrchardCore.Queries.GraphQL.Controllers;
using Microsoft.AspNetCore.Routing;
using System;
using OrchardCore.Admin;
using OrchardCore.Mvc.Core.Utilities;
using Microsoft.Extensions.Options;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    /// <summary>
    /// These services are registered on the tenant service collection
    /// </summary>
    [Feature("StatCan.OrchardCore.Queries.GraphQL")]
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<IDisplayDriver<Query>, GraphQLQueryDisplayDriver>();
            services.AddScoped<IQuerySource, GraphQLQuerySource>();
            services.AddScoped<INavigationProvider, AdminMenu>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var adminControllerName = typeof(AdminController).ControllerName();

            routes.MapAreaControllerRoute(
                name: "QueriesRunGraphQL",
                areaName: "StatCan.OrchardCore.Queries.GraphQL",
                pattern: _adminOptions.AdminUrlPrefix + "/Queries/GraphQL/Query",
                defaults: new { controller = adminControllerName, action = nameof(AdminController.Query) }
            );
        }
    }
}
