using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.OpenId.Services;
using OrchardCore.Settings;
using StatCan.OrchardCore.SaaSConfiguration.ViewModels;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class SaaSConfigurationSettingsDisplayDriver : SectionDisplayDriver<ISite, SaaSConfigurationSettings>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOpenIdServerService _serverService;
        private readonly ISaaSConfigurationService _saasConfigurationService;

        public SaaSConfigurationSettingsDisplayDriver(
            IAuthorizationService authorizationService,
            IOpenIdServerService serverService,
            IHttpContextAccessor httpContextAccessor,
            ISaaSConfigurationService saasConfigurationService)
        {
            _authorizationService = authorizationService;
            _serverService = serverService;

            _httpContextAccessor = httpContextAccessor;
            _saasConfigurationService = saasConfigurationService;
        }

        public override async Task<IDisplayResult> EditAsync(SaaSConfigurationSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageSaaSConfiguration))
            {
                return null;
            }

            return Initialize<SaasConfigurationSettingsViewModel>("SassConfigurationSettings_Edit", model =>
            {
                model.Authority = settings.Authority?.AbsoluteUri;
                model.ClientId = settings.ClientId;
            }).Location("Content:2").OnGroup(Constants.Features.SaaSConfiguration);
        }

        public override async Task<IDisplayResult> UpdateAsync(SaaSConfigurationSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (!await _authorizationService.AuthorizeAsync(user, Permissions.ManageSaaSConfiguration))
            {
                return null;
            }

            if (context.GroupId == Constants.Features.SaaSConfiguration)
            {
                // set the new values for settings
                var previousClientId = settings.ClientId;
                var model = new SaasConfigurationSettingsViewModel();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                settings.Authority = !string.IsNullOrEmpty(model.Authority) ? new Uri(model.Authority, UriKind.Absolute) : null;
                settings.ClientId = model.ClientId;

                // replace the ClientSecret only if the user provides a secret
                if (!string.IsNullOrEmpty(model.ClientSecret))
                {
                    settings.ClientSecret = _saasConfigurationService.GetProtectedClientSecret(model.ClientSecret);
                }

                //Update the OpenID server settings with the new values
                var serverSettings = await _serverService.GetSettingsAsync();
                serverSettings.Authority = settings.Authority;
                await _serverService.UpdateSettingsAsync(serverSettings);

                await _saasConfigurationService.UpdateApplicationAsync(previousClientId, settings);
                await _saasConfigurationService.UpdateTenantsClientSettingsAsync(settings);
            }

            return await EditAsync(settings, context);
        }
    }
}
