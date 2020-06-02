using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.Matomo.Settings;
using StatCan.OrchardCore.Matomo.ViewModels;
using OrchardCore.Settings;
using System;
using Microsoft.Extensions.Localization;
using System.Net;

namespace StatCan.OrchardCore.Matomo.Drivers
{
    public class MatomoSettingsDisplayDriver : SectionDisplayDriver<ISite, MatomoSettings>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer S;

        public MatomoSettingsDisplayDriver(
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MatomoSettingsDisplayDriver> s)
        {
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            S = s;
        }

        public override async Task<IDisplayResult> EditAsync(MatomoSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !await _authorizationService.AuthorizeAsync(user, MatomoPermissions.ManageMatomoSettings))
            {
                return null;
            }

            return Initialize<MatomoSettingsViewModel>("MatomoSettings_Edit", model =>
            {
                model.SiteID = settings.SiteID;
                model.ServerUri = settings.ServerUri;
            }).Location("Content:5").OnGroup(MatomoConstants.Features.Matomo);
        }

        public override async Task<IDisplayResult> UpdateAsync(MatomoSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == MatomoConstants.Features.Matomo)
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !await _authorizationService.AuthorizeAsync(user, MatomoPermissions.ManageMatomoSettings))
                {
                    return null;
                }

                var model = new MatomoSettingsViewModel();
                await context.Updater.TryUpdateModelAsync(model, Prefix);

                if (model.ServerUri.StartsWith("http://") ||
                    model.ServerUri.StartsWith("https://") ||
                    model.ServerUri.StartsWith("://") ||
                    !Uri.IsWellFormedUriString(model.ServerUri, UriKind.RelativeOrAbsolute))
                {
                    context.Updater.ModelState.AddModelError(Prefix, S["Please enter a valid server url."]);
                }

                if (context.Updater.ModelState.IsValid)
                {
                    settings.SiteID = model.SiteID;
                    // remove trailing / as it's added when injecting the script
                    settings.ServerUri = model.ServerUri.TrimEnd('/');
                }
            }
            return await EditAsync(settings, context);
        }
    }
}
