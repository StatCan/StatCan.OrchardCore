using Octokit;
using StatCan.OrchardCore.GitHub.Settings;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.GitHub.Services
{
    public interface IGitHubApiService
    {
        Task<GitHubClient> GetGitHubClient(string name);
        Task<ApiToken[]> GetTokens();
    }
}