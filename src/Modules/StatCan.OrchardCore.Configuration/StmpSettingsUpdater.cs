using System;
using System.Threading.Tasks;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using OrchardCore.Email;
using OrchardCore.Email.Services;
using Microsoft.AspNetCore.DataProtection;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrchardCore.Modules;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace StatCan.OrchardCore.Configuration
{
    /// <summary>
    /// Updates the SMTP settings based on a configuration
    /// </summary>
    public class SmtpSettingsUpdater : ModularTenantEvents, IFeatureEventHandler
    {
        private readonly ShellSettings _shellSettings;
        private readonly IShellHost _shellHost;
        private readonly ISiteService _siteService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IShellConfiguration _shellConfiguration;
        private readonly ILogger _logger;

        public SmtpSettingsUpdater(
            ISiteService siteService,
            IDataProtectionProvider dataProtectionProvider,
            IShellConfiguration shellConfiguration,
            ILogger<SmtpSettingsUpdater> logger,
            ShellSettings shellSettings,
            IShellHost shellHost)
        {
            _siteService = siteService;
            _dataProtectionProvider = dataProtectionProvider;
            _shellConfiguration = shellConfiguration;
            _logger = logger;
            _shellSettings = shellSettings;
            _shellHost = shellHost;
        }

        // for existing sites, update settings from configuration once
        public async override Task ActivatedAsync()
        {
            var section = _shellConfiguration.GetSection("StatCan_Configuration");

            if(section.GetValue<bool>("OverwriteSmtpSettings"))
            {
                await UpdateConfiguration();
            }
        }

        Task IFeatureEventHandler.InstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.InstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnabledAsync(IFeatureInfo feature) => SetConfiguredSmtpSettings(feature);

        Task IFeatureEventHandler.DisablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.DisabledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.UninstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.UninstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        private async Task SetConfiguredSmtpSettings(IFeatureInfo feature)
        {
            if (feature.Id != "OrchardCore.Email")
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
            var smtpSettings = siteSettings.As<SmtpSettings>();
            var section = _shellConfiguration.GetSection("StatCan_OrchardCore_Smtp");

            smtpSettings.DefaultSender = section.GetValue("DefaultSender", string.Empty);

            smtpSettings.PickupDirectoryLocation = section.GetValue("PickupDirectoryLocation", string.Empty);
            smtpSettings.Host = section.GetValue("Host", string.Empty);
            smtpSettings.Port = section.GetValue("Port", 25);

            smtpSettings.AutoSelectEncryption = section.GetValue("AutoSelectEncryption", false);
            smtpSettings.RequireCredentials = section.GetValue("RequireCredentials", false);
            smtpSettings.UseDefaultCredentials = section.GetValue("UseDefaultCredentials", false);
            smtpSettings.UserName = section.GetValue("UserName", string.Empty);

            var deliveryMethod = section.GetValue("DeliveryMethod", string.Empty);
            if(!string.IsNullOrEmpty(deliveryMethod))
            {
                if(Enum.TryParse(deliveryMethod, out SmtpDeliveryMethod method))
                {
                    smtpSettings.DeliveryMethod = method;
                }
            }

            var encryptionMethod = section.GetValue("EncryptionMethod", string.Empty);
            if(!string.IsNullOrEmpty(encryptionMethod))
            {
                if(Enum.TryParse(encryptionMethod, out SmtpEncryptionMethod method))
                {
                    smtpSettings.EncryptionMethod = method;
                }
            }

            var password = section.GetValue("Password", string.Empty);

            if (!string.IsNullOrEmpty(password))
            {
                try
                {
                    var protector = _dataProtectionProvider.CreateProtector(nameof(SmtpSettingsConfiguration));
                    smtpSettings.Password = protector.Protect(password);
                }
                catch
                {
                    _logger.LogError("The Smtp password could not be encrypted.");
                }
            }
            siteSettings.Put(smtpSettings);
        }

        private static bool NeedsUpdate(ISite settings)
        {
            var configSettings = settings.As<ConfigurationSettings>();
            if(!string.IsNullOrEmpty(configSettings.SmtpSettingsHash))
            {
                var encValue = EncryptSettings(settings);
                if(configSettings.SmtpSettingsHash == encValue)
                {
                    return false;
                }
            }
            return true;
        }

        private static void SetHash(ISite settings)
        {
            var configSettings = settings.As<ConfigurationSettings>();
            configSettings.SmtpSettingsHash = EncryptSettings(settings);
            settings.Put(configSettings);
        }

        private static string EncryptSettings(ISite settings)
        {
            using var sha256 = SHA256.Create();
            if (settings.Properties.TryGetValue(nameof(SmtpSettings), out var value))
            {
                return WebEncoders.Base64UrlEncode(sha256.ComputeHash(Encoding.UTF8.GetBytes(value.ToString())));
            }
            return string.Empty;
        }
    }
}
