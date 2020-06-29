using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StatCan.OrchardCore.GCCollab.Services;
using StatCan.OrchardCore.GCCollab.Settings;

namespace StatCan.OrchardCore.GCCollab.Configuration
{
    public class GCCollabOptionsConfiguration :
        IConfigureOptions<AuthenticationOptions>,
        IConfigureNamedOptions<GCCollabOptions>
    {
        private readonly IGCCollabAuthenticationService _gccollabAuthenticationService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly ILogger<GCCollabOptionsConfiguration> _logger;

        public GCCollabOptionsConfiguration(
            IGCCollabAuthenticationService gcCollabAuthenticationService,
            IDataProtectionProvider dataProtectionProvider,
            ILogger<GCCollabOptionsConfiguration> logger)
        {
            _gccollabAuthenticationService = gcCollabAuthenticationService;
            _dataProtectionProvider = dataProtectionProvider;
            _logger = logger;
        }

        public void Configure(AuthenticationOptions options)
        {
            var settings = GetGCCollabAuthenticationSettingsAsync().GetAwaiter().GetResult();
            if (settings == null)
            {
                return;
            }

            if (_gccollabAuthenticationService.ValidateSettings(settings).Any())
                return;

            // Register the OpenID Connect client handler in the authentication handlers collection.
            options.AddScheme(GCCollabDefaults.AuthenticationScheme, builder =>
            {
                builder.DisplayName = "GCCollab";
                builder.HandlerType = typeof(GCCollabHandler);
            });
        }

        public void Configure(string name, GCCollabOptions options)
        {
            // Ignore OpenID Connect client handler instances that don't correspond to the instance managed by the OpenID module.
            if (!string.Equals(name, GCCollabDefaults.AuthenticationScheme))
            {
                return;
            }

            var loginSettings = GetGCCollabAuthenticationSettingsAsync().GetAwaiter().GetResult();

            options.ClientId = loginSettings?.ClientID ?? string.Empty;

            try
            {
                options.ClientSecret = _dataProtectionProvider.CreateProtector(GCCollabConstants.Features.GCCollabAuthentication).Unprotect(loginSettings.ClientSecret);
            }
            catch
            {
                _logger.LogError("The GCCollab client secret key could not be decrypted. It may have been encrypted using a different key.");
            }

            if (loginSettings.CallbackPath.HasValue)
            {
                options.CallbackPath = loginSettings.CallbackPath;
            }
        }

        public void Configure(GCCollabOptions options) => Debug.Fail("This infrastructure method shouldn't be called.");

        private async Task<GCCollabAuthenticationSettings> GetGCCollabAuthenticationSettingsAsync()
        {
            var settings = await _gccollabAuthenticationService.GetSettingsAsync();
            if ((_gccollabAuthenticationService.ValidateSettings(settings)).Any(result => result != ValidationResult.Success))
            {
                _logger.LogWarning("The GCCollab Authentication is not correctly configured.");

                return null;
            }
            return settings;
        }
    }
}
