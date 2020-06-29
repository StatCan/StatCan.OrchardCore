using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Environment.Shell;
using StatCan.OrchardCore.GCCollab.Settings;
using StatCan.OrchardCore.GCCollab.ViewModels;
using OrchardCore.Settings;

namespace StatCan.OrchardCore.GCCollab.Drivers
{
    public class GCCollabAuthenticationSettingsDisplayDriver : SectionDisplayDriver<ISite, GCCollabAuthenticationSettings>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShellHost _shellHost;
        private readonly ShellSettings _shellSettings;

        public GCCollabAuthenticationSettingsDisplayDriver(
            IAuthorizationService authorizationService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            IShellHost shellHost,
            ShellSettings shellSettings)
        {
            _authorizationService = authorizationService;
            _dataProtectionProvider = dataProtectionProvider;
            _httpContextAccessor = httpContextAccessor;
            _shellHost = shellHost;
            _shellSettings = shellSettings;
        }

        public override async Task<IDisplayResult> EditAsync(GCCollabAuthenticationSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !await _authorizationService.AuthorizeAsync(user, Permissions.ManageGCCollabAuthentication))
            {
                return null;
            }

            return Initialize<GCCollabAuthenticationSettingsViewModel>("GCCollabAuthenticationSettings_Edit", model =>
            {
                model.ClientID = settings.ClientID;
                if (!string.IsNullOrWhiteSpace(settings.ClientSecret))
                {
                    var protector = _dataProtectionProvider.CreateProtector(GCCollabConstants.Features.GCCollabAuthentication);
                    model.ClientSecret = protector.Unprotect(settings.ClientSecret);
                }
                else
                {
                    model.ClientSecret = string.Empty;
                }
                if (settings.CallbackPath.HasValue)
                {
                    model.CallbackUrl = settings.CallbackPath.Value;
                }
            }).Location("Content:5").OnGroup(GCCollabConstants.Features.GCCollabAuthentication);
        }

        public override async Task<IDisplayResult> UpdateAsync(GCCollabAuthenticationSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == GCCollabConstants.Features.GCCollabAuthentication)
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !await _authorizationService.AuthorizeAsync(user, Permissions.ManageGCCollabAuthentication))
                {
                    return null;
                }

                var model = new GCCollabAuthenticationSettingsViewModel();
                await context.Updater.TryUpdateModelAsync(model, Prefix);

                if (context.Updater.ModelState.IsValid)
                {
                    var protector = _dataProtectionProvider.CreateProtector(GCCollabConstants.Features.GCCollabAuthentication);

                    settings.ClientID = model.ClientID;
                    settings.ClientSecret = protector.Protect(model.ClientSecret);
                    settings.CallbackPath = model.CallbackUrl;
                    await _shellHost.ReloadShellContextAsync(_shellSettings);
                }
            }
            return await EditAsync(settings, context);
        }
    }
}
