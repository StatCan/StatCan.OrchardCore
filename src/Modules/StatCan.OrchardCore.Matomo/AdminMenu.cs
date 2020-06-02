using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.Matomo
{
    public class MatomoAdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public MatomoAdminMenu(IStringLocalizer<MatomoAdminMenu> localizer)
        {
            S = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                builder.Add(S["Configuration"], configuration => configuration
                        .Add(S["Settings"], settings => settings
                            .Add(S["Matomo"], S["Matomo"].PrefixPosition(), settings => settings
                            .AddClass("matomo").Id("matomo")
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = MatomoConstants.Features.Matomo })
                                .Permission(MatomoPermissions.ManageMatomoSettings)
                                .LocalNav())
                            )
                        );
            }
            return Task.CompletedTask;
        }
    }
}
