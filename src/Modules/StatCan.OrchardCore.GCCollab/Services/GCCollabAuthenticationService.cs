using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Entities;
using StatCan.OrchardCore.GCCollab.Settings;
using OrchardCore.Settings;

namespace StatCan.OrchardCore.GCCollab.Services
{
    public class GCCollabAuthenticationService : IGCCollabAuthenticationService
    {
        private readonly ISiteService _siteService;
        private readonly IStringLocalizer<GCCollabAuthenticationService> S;

        public GCCollabAuthenticationService(
            ISiteService siteService,
            IStringLocalizer<GCCollabAuthenticationService> stringLocalizer)
        {
            _siteService = siteService;
            S = stringLocalizer;
        }

        public async Task<GCCollabAuthenticationSettings> GetSettingsAsync()
        {
            var container = await _siteService.GetSiteSettingsAsync();
            return container.As<GCCollabAuthenticationSettings>();
        }

        public async Task UpdateSettingsAsync(GCCollabAuthenticationSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            var container = await _siteService.LoadSiteSettingsAsync();
            container.Alter<GCCollabAuthenticationSettings>(nameof(GCCollabAuthenticationSettings), aspect =>
            {
                aspect.ClientID = settings.ClientID;
                aspect.ClientSecret = settings.ClientSecret;
                aspect.CallbackPath = settings.CallbackPath;
            });
            await _siteService.UpdateSiteSettingsAsync(container);
        }

        public IEnumerable<ValidationResult> ValidateSettings(GCCollabAuthenticationSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(settings.ClientID))
            {
                yield return new ValidationResult(S["ClientID is required"], new string[] { nameof(settings.ClientID) });
            }

            if (string.IsNullOrWhiteSpace(settings.ClientSecret))
            {
                yield return new ValidationResult(S["ClientSecret is required"], new string[] { nameof(settings.ClientSecret) });
            }
        }

    }
}
