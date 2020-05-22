using Fluid;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Liquid;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.Drivers;
using StatCan.OrchardCore.GitHub.Liquid;
using StatCan.OrchardCore.GitHub.LiquidModel;
using StatCan.OrchardCore.GitHub.Services;

namespace StatCan.OrchardCore.GitHub
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            // register settings
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<IDisplayDriver<ISite>, GitHubApiSettingsDisplayDriver>();
            services.AddScoped<INavigationProvider, GitHubApiAdminMenu>();

            services.AddScoped<IGitHubApiService, GitHubApiService>();

            services.AddActivity<CreateBranchTask, CreateBranchTaskDisplayDriver>();
            services.AddActivity<CommitFileTask, CommitFileTaskDisplayDriver>();
            services.AddActivity<CreatePullRequestTask, CreatePullRequestTaskDisplayDriver>();
            services.AddActivity<CreateIssueTask, CreateIssueTaskDisplayDriver>();

        }
    }

    [RequireFeatures("OrchardCore.Liquid")]
    public class LiquidStartup : StartupBase
    {
        static LiquidStartup()
        {
            // allow liquid to access the properties of these objects
            TemplateContext.GlobalMemberAccessStrategy.Register<GitReference>();
            TemplateContext.GlobalMemberAccessStrategy.Register<PullRequest>();
            TemplateContext.GlobalMemberAccessStrategy.Register<PullRequestReviewComment>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Reference>();
            TemplateContext.GlobalMemberAccessStrategy.Register<TagObject>();
            TemplateContext.GlobalMemberAccessStrategy.Register<RepositoryContentChangeSet>();
            TemplateContext.GlobalMemberAccessStrategy.Register<RepositoryContentInfo>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Commit>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Committer>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ErrorResult>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Issue>();
            TemplateContext.GlobalMemberAccessStrategy.Register<IssueComment>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Label>();
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLiquidFilter<GetIssueFilter>("github_issue");
            services.AddLiquidFilter<GetIssueCommentsFilter>("github_comments");
            services.AddLiquidFilter<GetPullRequestFilter>("github_pr");
            services.AddLiquidFilter<GetPullRequestCommentsFilter>("github_pr_reviewcomments");
        }
    }
}