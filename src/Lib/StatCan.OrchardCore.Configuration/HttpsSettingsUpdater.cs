using System.Threading.Tasks;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;
using OrchardCore.Modules;
using OrchardCore.Https.Settings;

namespace StatCan.OrchardCore.Configuration
{
    /// <summary>
    /// Updates the Https settings based on a configuration
    /// </summary>
    public class HttpsSettingsUpdater : ModularTenantEvents, IFeatureEventHandler
    {
        private readonly ISiteService _siteService;
        private readonly IShellConfiguration _shellConfiguration;

        public HttpsSettingsUpdater(
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

            if(section.GetValue<bool>("OverwriteHttpsSettings"))
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
            if (feature.Id != "OrchardCore.Https")
            {
                return;
            }
            var siteSettings = await _siteService.LoadSiteSettingsAsync();
            SetConfiguration(siteSettings);
            await _siteService.UpdateSiteSettingsAsync(siteSettings);
        }

        private void SetConfiguration(ISite siteSettings)
        {
            var httpsSettings = siteSettings.As<HttpsSettings>();
            var section = _shellConfiguration.GetSection("StatCan_Https");

            httpsSettings.RequireHttps = section.GetValue("RequireHttps", false);
            httpsSettings.EnableStrictTransportSecurity = section.GetValue("EnableStrictTransportSecurity", false);
            httpsSettings.RequireHttpsPermanent = section.GetValue("RequireHttpsPermanent", false);
            httpsSettings.SslPort = section.GetValue<int?>("SslPort", null);

            siteSettings.Put(httpsSettings);
        }
    }
}
