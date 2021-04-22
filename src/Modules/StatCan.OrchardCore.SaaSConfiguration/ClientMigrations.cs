using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OpenIddict.Abstractions;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.OpenId.Configuration;
using OrchardCore.OpenId.Services;
using OrchardCore.Settings;
using OrchardCore.Setup.Events;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class ClientMigrations : DataMigration
    {
        private readonly IShellHost _shellHost;
        private readonly ITenantHelperService _tenantHelper;
        private readonly IOpenIdClientService _openIdClientService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ShellSettings _shellSettings;
        private readonly INotifier _notifier;
        private readonly IHtmlLocalizer H;

        public ClientMigrations(IShellHost shellHost,
            ITenantHelperService tenantHelper,
            IOpenIdClientService openIdClientService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            ShellSettings shellSettings,
            INotifier notifier,
            IHtmlLocalizer<ClientMigrations> htmlLocalizer)
        {
            _shellHost = shellHost;
            _tenantHelper = tenantHelper;
            _openIdClientService = openIdClientService;
            _dataProtectionProvider = dataProtectionProvider;
            _httpContextAccessor = httpContextAccessor;
            _shellSettings = shellSettings;
            _notifier = notifier;
            H = htmlLocalizer;
        }

        public async Task<int> CreateAsync()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            // Get the Configuration options from the default tenant
            bool moduleEnabled = true;
            SaaSConfigurationSettings saasConfigurationSettings = null;
            string unprotectedSecret = null;
            Uri authority = null;

            var currentUri = new UriBuilder()
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port ?? 0,
                Path = _shellSettings.RequestUrlPrefix + "/signin-oidc"
            };

            var shellScope = await _shellHost.GetScopeAsync(ShellHelper.DefaultShellName);
            // This code is running on the default tenant to get the configuration settings for this tenant
            await shellScope.UsingAsync(async scope =>
            {
                var extensionManager = scope.ServiceProvider.GetRequiredService<IExtensionManager>();
                var feature = extensionManager.GetFeatures().FirstOrDefault(f => f.Id == Constants.Features.SaaSConfiguration);
                // tenant does not have our ConfigurationClient enabled, thus not configured by this module.
                if(feature == null)
                {
                    moduleEnabled = false;
                    return;
                }

                var saasConfigurationService = scope.ServiceProvider.GetRequiredService<ISaaSConfigurationService>();

                saasConfigurationSettings = await saasConfigurationService.GetSettingsAsync();
                unprotectedSecret = saasConfigurationService.GetUnprotectedClientSecret(saasConfigurationSettings.ClientSecret);
                authority = saasConfigurationSettings.Authority;

                await saasConfigurationService.UpdateRedirectUris(currentUri.ToString());
            });

            if(!moduleEnabled)
            {
                await _tenantHelper.DisableFeatureAsync(Constants.Features.SaaSConfigurationClient);
                _notifier.Error(H["The Default tenant does not have the {0} feature enabled. The {1} module has been disabled as a result", Constants.Features.SaaSConfiguration, Constants.Features.SaaSConfigurationClient]);
                return 0 ;
            }

            var settings = await _openIdClientService.GetSettingsAsync();

            settings.Authority = authority;
            settings.ClientId = saasConfigurationSettings.ClientId;
            settings.Scopes = new string[]{OpenIddictConstants.Scopes.Email};
            settings.DisplayName = "Login"; //todo i18n?
            settings.ResponseType = OpenIdConnectResponseType.Code;
            settings.ResponseMode = OpenIdConnectResponseMode.FormPost;

            // protect the client secret using the openid protector.
            var clientProtector = _dataProtectionProvider.CreateProtector(nameof(OpenIdClientConfiguration));
            settings.ClientSecret = clientProtector.Protect(unprotectedSecret);

            var results = await _openIdClientService.ValidateSettingsAsync(settings);

            await _openIdClientService.UpdateSettingsAsync(settings);

            // restart the tenant to reload the OpenId connect configuration
            await _shellHost.ReleaseShellContextAsync(_shellSettings);

            return 1;
        }
    }
}
