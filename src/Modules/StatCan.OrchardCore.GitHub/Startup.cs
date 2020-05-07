using Fluid;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using OrchardCore.Workflows.Helpers;
using StatCan.OrchardCore.GitHub.Activities;
using StatCan.OrchardCore.GitHub.Drivers;
using StatCan.OrchardCore.GitHub.LiquidModel;
using StatCan.OrchardCore.GitHub.Services;

namespace StatCan.OrchardCore.GitHub
{
    public class Startup : StartupBase
    {
        static Startup()
        {
            // allow liquid to access the properties of these objects
            TemplateContext.GlobalMemberAccessStrategy.Register<GitReference>();
            TemplateContext.GlobalMemberAccessStrategy.Register<PullRequest>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Reference>();
            TemplateContext.GlobalMemberAccessStrategy.Register<TagObject>();
            TemplateContext.GlobalMemberAccessStrategy.Register<RepositoryContentChangeSet>();
            TemplateContext.GlobalMemberAccessStrategy.Register<RepositoryContentInfo>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Commit>();
            TemplateContext.GlobalMemberAccessStrategy.Register<Committer>();
            TemplateContext.GlobalMemberAccessStrategy.Register<ErrorResult>();

        }
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

        }
    }
}