using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using StatCan.OrchardCore.GitHub.Settings;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class GitHubApiSettingsDisplayDriver : SectionDisplayDriver<ISite, GitHubApiSettings>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GitHubApiSettingsDisplayDriver(
            IAuthorizationService authorizationService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor)
        {
            _authorizationService = authorizationService;
            _dataProtectionProvider = dataProtectionProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task<IDisplayResult> EditAsync(GitHubApiSettings settings, BuildEditorContext context)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !await _authorizationService.AuthorizeAsync(user, Permissions.ManageGitHubApiSettings))
            {
                return null;
            }

            return Initialize<GitHubApiSettingsViewModel>("GitHubApiSettings_Edit", model =>
            {
                if (!string.IsNullOrWhiteSpace(settings.ApiToken))
                {
                    var protector = _dataProtectionProvider.CreateProtector(GitHubConstants.Features.GitHub);
                    model.ApiToken = protector.Unprotect(settings.ApiToken);
                }
                else
                {
                    model.ApiToken = string.Empty;
                }
            }).Location("Content:5").OnGroup(GitHubConstants.Features.GitHub);
        }

        public override async Task<IDisplayResult> UpdateAsync(GitHubApiSettings settings, BuildEditorContext context)
        {
            if (context.GroupId == GitHubConstants.Features.GitHub)
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !await _authorizationService.AuthorizeAsync(user, Permissions.ManageGitHubApiSettings))
                {
                    return null;
                }

                var model = new GitHubApiSettingsViewModel();
                await context.Updater.TryUpdateModelAsync(model, Prefix);

                if (context.Updater.ModelState.IsValid)
                {
                    var protector = _dataProtectionProvider.CreateProtector(GitHubConstants.Features.GitHub);

                    settings.ApiToken = protector.Protect(model.ApiToken);
                }
            }
            return await EditAsync(settings, context);
        }
    }
}
