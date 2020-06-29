using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Navigation;
using StatCan.OrchardCore.GCCollab.Configuration;
using StatCan.OrchardCore.GCCollab.Drivers;
using StatCan.OrchardCore.GCCollab.Services;
using OrchardCore.Modules;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using OrchardCore.Recipes;
using StatCan.OrchardCore.GCCollab.Recipes;

namespace StatCan.OrchardCore.GCCollab
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IPermissionProvider, Permissions>();
        }
    }

    [Feature(GCCollabConstants.Features.GCCollabAuthentication)]
    public class GCCollabLoginStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IGCCollabAuthenticationService, GCCollabAuthenticationService>();
            services.AddScoped<IDisplayDriver<ISite>, GCCollabAuthenticationSettingsDisplayDriver>();
            services.AddScoped<INavigationProvider, AdminMenuGCCollabLogin>();
            services.AddRecipeExecutionStep<GCCollabAuthenticationSettingsStep>();
            // Register the options initializers required by the GCCollab Handler.
            services.TryAddEnumerable(new[]
            {
                // Orchard-specific initializers:
                ServiceDescriptor.Transient<IConfigureOptions<AuthenticationOptions>, GCCollabOptionsConfiguration>(),
                ServiceDescriptor.Transient<IConfigureOptions<GCCollabOptions>, GCCollabOptionsConfiguration>(),
                // Built-in initializers:
                ServiceDescriptor.Transient<IPostConfigureOptions<GCCollabOptions>, OAuthPostConfigureOptions<GCCollabOptions,GCCollabHandler>>()
            });
        }
    }
}
