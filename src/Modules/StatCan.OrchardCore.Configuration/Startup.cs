using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Configuration
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PersistentFeatures>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<PersistentFeatures>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<PersistentFeatures>());

            services.AddScoped<SmtpSettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<SmtpSettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<SmtpSettingsUpdater>());

            services.AddScoped<ReverseProxySettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<ReverseProxySettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<ReverseProxySettingsUpdater>());

            services.AddScoped<HttpsSettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<HttpsSettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<HttpsSettingsUpdater>());
        }
    }
}

















