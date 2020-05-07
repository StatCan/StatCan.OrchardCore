using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Modules;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.GitHub
{
    public class GitHubApiAdminMenu : INavigationProvider
    {
        private readonly ShellDescriptor _shellDescriptor;
        private readonly IStringLocalizer S;

        public GitHubApiAdminMenu(
            IStringLocalizer<GitHubApiAdminMenu> localizer,
            ShellDescriptor shellDescriptor)
        {
            S = localizer;
            _shellDescriptor = shellDescriptor;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                builder.Add(S["Configuration"], configuration => configuration
                        .Add(S["Settings"], settings => settings
                            .Add(S["GitHub Api"], S["GitHub Api"].PrefixPosition(), settings => settings
                            .AddClass("githubapi").Id("githubapi")
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = GitHubConstants.Features.GitHub })
                                .Permission(Permissions.ManageGitHubApiSettings)
                                .LocalNav())
                            )
                        );
            }
            return Task.CompletedTask;
        }
    }
}
