using Octokit;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.GitHub.Services
{
    public interface IGitHubApiService
    {
        Task<GitHubClient> GetGitHubClient(string name);
    }
}