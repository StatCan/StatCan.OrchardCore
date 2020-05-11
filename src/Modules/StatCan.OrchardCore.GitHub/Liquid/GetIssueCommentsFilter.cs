using System;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using OrchardCore.Liquid;
using StatCan.OrchardCore.GitHub.Services;

namespace StatCan.OrchardCore.GitHub.Liquid
{
    public class GetIssueCommentsFilter : ILiquidFilter
    {
        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            if (!ctx.AmbientValues.TryGetValue("Services", out var services))
            {
                throw new ArgumentException("Services missing while invoking 'github_issuecomments'");
            }

            var gitHubApiService = ((IServiceProvider)services).GetRequiredService<IGitHubApiService>();

            var owner = arguments["owner"].Or(arguments.At(0)).ToStringValue();
            var repo = arguments["repository"].Or(arguments.At(1)).ToStringValue();
            var prNumber = input.ToStringValue();

            if(string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo))
            {
                throw new ArgumentException("Missing owner, repository while invoking 'github_issuecomments'");
            }

            if(!int.TryParse(prNumber, out var parsedNumber))
            {
                throw new ArgumentException("Please provide a valid pull request number while invoking 'github_issuecomments'");
            }

            try
            {
                var client = await gitHubApiService.GetGitHubClient();
                return FluidValue.Create(await client.Issue.Comment.GetAllForIssue(owner, repo, parsedNumber));
            }
            catch(Octokit.ApiException ex)
            {
                return FluidValue.Create(ex.Message);
            }
        }
    }
}
