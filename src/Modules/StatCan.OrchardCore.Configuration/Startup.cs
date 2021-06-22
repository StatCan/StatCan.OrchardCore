using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using OrchardCore.Modules;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;

using Email = OrchardCore.Email;
using ReverseProxy = OrchardCore.ReverseProxy;
using Https = OrchardCore.Https;
namespace StatCan.OrchardCore.Configuration
{
    [RequireFeatures("OrchardCore.Email")]
    public class EmailStartup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        public EmailStartup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<SmtpSettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<SmtpSettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<SmtpSettingsUpdater>());

            if(_shellConfiguration.GetSection("StatCan_Configuration").GetValue<bool>("OverwriteSmtpSettings"))
            {
                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(Email.AdminMenu));
                services.Remove(serviceDescriptor);
            }
        }
    }

    [RequireFeatures("OrchardCore.ReverseProxy")]
    public class ReverseProxyStartup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        public ReverseProxyStartup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ReverseProxySettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<ReverseProxySettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<ReverseProxySettingsUpdater>());
            if(_shellConfiguration.GetSection("StatCan_Configuration").GetValue<bool>("OverwriteReverseProxySettings"))
            {
                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(ReverseProxy.AdminMenu));
                services.Remove(serviceDescriptor);
            }
        }
    }
    [RequireFeatures("OrchardCore.Https")]
    public class HttpsStartup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        public HttpsStartup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<HttpsSettingsUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<HttpsSettingsUpdater>());
            services.AddScoped<IModularTenantEvents>(sp => sp.GetRequiredService<HttpsSettingsUpdater>());

            if(_shellConfiguration.GetSection("StatCan_Configuration").GetValue<bool>("OverwriteHttpsSettings"))
            {
                var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(Https.AdminMenu));
                services.Remove(serviceDescriptor);
            }
        }
    }
}
