using Microsoft.AspNetCore.DataProtection;
using Octokit;
using OrchardCore.Entities;
using OrchardCore.Settings;
using StatCan.OrchardCore.GitHub.Settings;
using System;
using System.Collections.Generic;
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
        public async Task<GitHubClient> GetGitHubClient()
        {
            var encToken = (await _siteService.GetSiteSettingsAsync()).As<GitHubApiSettings>().ApiToken;

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
    }
}
