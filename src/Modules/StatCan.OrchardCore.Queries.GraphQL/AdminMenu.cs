using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
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
                .Add(S["Search"], search => search
                    .Add(S["Queries"], S["Queries"].PrefixPosition(), queries => queries
                        .Add(S["Run GraphQL Query"], S["Run GraphQL Query"].PrefixPosition(), sql => sql
                             .Action("Query", "Admin", new { area = "StatCan.OrchardCore.Queries.GraphQL" })
                             .Permission(Permissions.ManageGraphQLQueries)
                             .LocalNav())));

            return Task.CompletedTask;
        }
    }
}
