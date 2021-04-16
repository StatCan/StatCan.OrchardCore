using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(
            IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(S["Configuration"], configuration => configuration
                    .Add(S["SaaS"], security => security

                        .Add(S["OpenId"], S["OpenId"].PrefixPosition(), openid =>
                        {
                            openid.Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = Constants.Features.SaaSConfiguration })
                            .Permission(Permissions.ManageSaaSConfiguration)
                            .LocalNav();
                        })
                        .Add(S["Execute JSON"], S["Execute JSON"].PrefixPosition(), deployment => deployment
                            .Action("ExecuteRecipe", "Admin", Constants.Features.SaaSConfiguration)
                            .Permission(Permissions.ManageSaaSConfiguration)
                            .LocalNav()
                        )
                    )
                );

            return Task.CompletedTask;
        }
    }
}
