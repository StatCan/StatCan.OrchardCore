using System.Threading.Tasks;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;
using OrchardCore.Modules;
using OrchardCore.Https.Settings;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace StatCan.OrchardCore.Configuration
{
    /// <summary>
    /// Updates the Https settings based on a configuration
    /// </summary>
    public class HttpsSettingsUpdater : ModularTenantEvents, IFeatureEventHandler
    {
        private readonly ShellSettings _shellSettings;
        private readonly IShellHost _shellHost;
        private readonly ISiteService _siteService;
        private readonly IShellConfiguration _shellConfiguration;

        public HttpsSettingsUpdater(
            ISiteService siteService,
            IShellConfiguration shellConfiguration,
            IShellHost shellHost,
            ShellSettings shellSettings)
        {
            _siteService = siteService;
            _shellConfiguration = shellConfiguration;
            _shellHost = shellHost;
            _shellSettings = shellSettings;
        }

        // for existing sites, update settings from configuration when tenant activates
        public async override Task ActivatedAsync()
        {
            var section = _shellConfiguration.GetSection("StatCan_Configuration");

            if(section.GetValue<bool>("OverwriteHttpsSettings"))
            {
               await UpdateConfiguration();
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
            await UpdateConfiguration();
        }

        private async Task UpdateConfiguration()
        {
            var siteSettings = await _siteService.LoadSiteSettingsAsync();
            if(NeedsUpdate(siteSettings))
            {
                SetConfiguration(siteSettings);
                SetHash(siteSettings);
                await _siteService.UpdateSiteSettingsAsync(siteSettings);
                await _shellHost.ReleaseShellContextAsync(_shellSettings);
            }
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

        private static bool NeedsUpdate(ISite settings)
        {
            var configSettings = settings.As<ConfigurationSettings>();
            if(!string.IsNullOrEmpty(configSettings.HttpsSettingsHash))
            {
                var encValue = EncryptSettings(settings);
                if(configSettings.HttpsSettingsHash == encValue)
                {
                    return false;
                }
            }
            return true;
        }

        private static void SetHash(ISite settings)
        {
            var configSettings = settings.As<ConfigurationSettings>();
            configSettings.HttpsSettingsHash = EncryptSettings(settings);
            settings.Put(configSettings);
        }

        private static string EncryptSettings(ISite settings)
        {
            using var sha256 = SHA256.Create();
            if (settings.Properties.TryGetValue(nameof(HttpsSettings), out var value))
            {
                return WebEncoders.Base64UrlEncode(sha256.ComputeHash(Encoding.UTF8.GetBytes(value.ToString())));
            }
            return string.Empty;
        }
    }
}
