using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Handlers;
using StatCan.OrchardCore.Matomo.Drivers;
using StatCan.OrchardCore.Matomo.Recipes;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Recipes;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;

namespace StatCan.OrchardCore.Matomo
{
    public class MatomoStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, MatomoPermissions>();
            services.AddRecipeExecutionStep<MatomoSettingsStep>();
            services.AddScoped<IDisplayDriver<ISite>, MatomoSettingsDisplayDriver>();
            services.AddScoped<INavigationProvider, MatomoAdminMenu>();
            services.Configure<MvcOptions>((options) =>
            {
                options.Filters.Add(typeof(MatomoFilter));
            });
        }
    }
}
