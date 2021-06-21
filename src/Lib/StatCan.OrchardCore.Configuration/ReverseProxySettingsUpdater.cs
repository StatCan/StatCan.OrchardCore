using System.Threading.Tasks;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;
using OrchardCore.Modules;
using OrchardCore.ReverseProxy.Settings;
using Microsoft.AspNetCore.HttpOverrides;

namespace StatCan.OrchardCore.Configuration
{
    /// <summary>
    /// Updates the Https settings based on a configuration
    /// </summary>
    public class ReverseProxySettingsUpdater : ModularTenantEvents, IFeatureEventHandler
    {
        private readonly ISiteService _siteService;
        private readonly IShellConfiguration _shellConfiguration;

        public ReverseProxySettingsUpdater(
            ISiteService siteService,
            IShellConfiguration shellConfiguration)
        {
            _siteService = siteService;
            _shellConfiguration = shellConfiguration;
        }

        // for existing sites, update settings from configuration when tenant activates
        public async override Task ActivatedAsync()
        {
            var section = _shellConfiguration.GetSection("StatCan_Configuration");

            if(section.GetValue<bool>("OverwriteReverseProxySettings"))
            {
                var siteSettings = await _siteService.LoadSiteSettingsAsync();
                SetConfiguration(siteSettings);
                await _siteService.UpdateSiteSettingsAsync(siteSettings);
            }
        }

        Task IFeatureEventHandler.InstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.InstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnabledAsync(IFeatureInfo feature) => SetConfiguredHttpsSettings(feature);

        Task IFeatureEventHandler.DisablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.DisabledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.UninstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.UninstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        private async Task SetConfiguredHttpsSettings(IFeatureInfo feature)
        {
            if (feature.Id != "OrchardCore.ReverseProxy")
            {
                return;
            }
            var siteSettings = await _siteService.LoadSiteSettingsAsync();
            SetConfiguration(siteSettings);
            await _siteService.UpdateSiteSettingsAsync(siteSettings);
        }

        private void SetConfiguration(ISite siteSettings)
        {
            var reverseProxySettings = siteSettings.As<ReverseProxySettings>();
            var section = _shellConfiguration.GetSection("StatCan_ReverseProxy");

            reverseProxySettings.ForwardedHeaders = ForwardedHeaders.None;

            if (section.GetValue("EnableXForwardedFor", false))
            {
                reverseProxySettings.ForwardedHeaders |= ForwardedHeaders.XForwardedFor;
            }

            if (section.GetValue("EnableXForwardedHost", false))
            {
                reverseProxySettings.ForwardedHeaders |= ForwardedHeaders.XForwardedHost;
            }
            if (section.GetValue("EnableXForwardedProto", false))
            {
                reverseProxySettings.ForwardedHeaders |= ForwardedHeaders.XForwardedProto;
            }

            siteSettings.Put(reverseProxySettings);
        }
    }
}
