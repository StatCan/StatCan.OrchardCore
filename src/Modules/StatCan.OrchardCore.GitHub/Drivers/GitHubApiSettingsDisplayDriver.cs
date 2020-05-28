using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.DisplayManagement.Entities;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Settings;
using Org.BouncyCastle.Crypto.Modes;
using StatCan.OrchardCore.GitHub.Settings;
using StatCan.OrchardCore.GitHub.ViewModels;

namespace StatCan.OrchardCore.GitHub.Drivers
{
    public class GitHubApiSettingsDisplayDriver : SectionDisplayDriver<ISite, GitHubApiSettings>
    {
        private static string MagicString = "<filled-token>";
        private readonly IStringLocalizer S;
        private readonly IAuthorizationService _authorizationService;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GitHubApiSettingsDisplayDriver(
            IAuthorizationService authorizationService,
            IDataProtectionProvider dataProtectionProvider,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<GitHubApiSettingsDisplayDriver> localizer)
        {
            _authorizationService = authorizationService;
            _dataProtectionProvider = dataProtectionProvider;
            _httpContextAccessor = httpContextAccessor;
            S = localizer;
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
                if (settings.ApiTokens != null)
                {
                    //map to a magic string so the api key is not leaked in the html
                    var sanitizedTokens = settings.ApiTokens.Select(t => new ApiToken() { Name = t.Name, Value = MagicString + t.Name });
                    model.ApiTokens = JsonConvert.SerializeObject(sanitizedTokens, Formatting.Indented);
                }
                else
                {
                    model.ApiTokens = JsonConvert.SerializeObject(new ApiToken[0], Formatting.Indented);
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

                    if(model.ApiTokens != null)
                    {
                        try
                        {
                            var tokens = JsonConvert.DeserializeObject<ApiToken[]>(model.ApiTokens);

                            if (tokens.GroupBy(x => x.Name.Trim()).Any(g => g.Count() > 1))
                            {
                                context.Updater.ModelState.AddModelError(Prefix, S["Please use unique token names"]);
                                return await EditAsync(settings, context);
                            }

                            foreach (var token in tokens)
                            {
                                token.Name = token.Name.Trim();
                                if (token.Value.StartsWith(MagicString))
                                {
                                    // get the previous name from the magic string
                                    var prevName = token.Value.Substring(MagicString.Length - 1);
                                    // set the value of the token to the value stored in the db. It should already be encrypted
                                    token.Value = settings.ApiTokens.FirstOrDefault(t => t.Name == prevName).Value;
                                }
                                else
                                {
                                    // encrypt the token
                                    token.Value = protector.Protect(token.Value);
                                }

                            }
                            settings.ApiTokens = tokens;
                        }
                        catch
                        {
                            context.Updater.ModelState.AddModelError(Prefix, S["The tokens are written in an incorrect format."]);
                            return await EditAsync(settings, context);
                        }
                    }
                    else
                    {
                        settings.ApiTokens = new ApiToken[0];
                    }
                }
            }
            return await EditAsync(settings, context);
        }
    }
}
