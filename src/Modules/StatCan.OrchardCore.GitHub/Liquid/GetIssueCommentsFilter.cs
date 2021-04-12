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
        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext ctx)
        {
            if (!ctx.AmbientValues.TryGetValue("Services", out var services))
            {
                throw new ArgumentException("Services missing while invoking 'github_issuecomments'");
            }

            var gitHubApiService = ((IServiceProvider)services).GetRequiredService<IGitHubApiService>();

            var owner = arguments["owner"].Or(arguments.At(0)).ToStringValue();
            var repo = arguments["repository"].Or(arguments.At(1)).ToStringValue();
            var tokenName = arguments["tokenName"].Or(arguments.At(2)).ToStringValue();
            var prNumber = input.ToStringValue();

            if(string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(repo) || string.IsNullOrEmpty(tokenName))
            {
                throw new ArgumentException("Missing owner, repository or tokenName while invoking 'github_issuecomments'");
            }

            if(!int.TryParse(prNumber, out var parsedNumber))
            {
                throw new ArgumentException("Please provide a valid pull request number while invoking 'github_issuecomments'");
            }

            try
            {
                var client = await gitHubApiService.GetGitHubClient(tokenName);
                return FluidValue.Create(await client.Issue.Comment.GetAllForIssue(owner, repo, parsedNumber), ctx.Options);
            }
            catch(Octokit.ApiException ex)
            {
                return FluidValue.Create(ex.Message, ctx.Options);
            }
        }
    }
}
