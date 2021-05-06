using Microsoft.AspNetCore.DataProtection;
using Octokit;
using OrchardCore.Entities;
using OrchardCore.Settings;
using StatCan.OrchardCore.GitHub.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.GitHub.Services
{
    public class GitHubApiService : IGitHubApiService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly ISiteService _siteService;

        public GitHubApiService(IDataProtectionProvider dataProtectionProvider, ISiteService siteService)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _siteService = siteService;
        }
        public async Task<GitHubClient> GetGitHubClient(string name)
        {
            var encToken = (await _siteService.GetSiteSettingsAsync()).As<GitHubApiSettings>()?.ApiTokens?.FirstOrDefault(t=>t.Name == name)?.Value;

            if (string.IsNullOrEmpty(encToken))
            {
                throw new ArgumentException("Missing GitHub ApiToken");
            }

            var protector = _dataProtectionProvider.CreateProtector(GitHubConstants.Features.GitHub);

            return new GitHubClient(new ProductHeaderValue("CreateBranchTask"))
            {
                Credentials = new Credentials(protector.Unprotect(encToken))
            };
        }
        public async Task<ApiToken[]> GetTokens()
        {
            return (await _siteService.GetSiteSettingsAsync()).As<GitHubApiSettings>()?.ApiTokens;
        }
    }
}
