using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.VueForms
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

            var rvd = new RouteValueDictionary
            {
                { "Area", "OrchardCore.Contents" },
                { "Options.SelectedContentType", "VueForm" },
                { "Options.CanCreateSelectedContentType", true }
            };

            builder.Add(S["Content"], design => design
                    .Add(S["Vue Forms"], S["Vue Forms"], menus => menus
                        .Action("List", "Admin", rvd)
                        .LocalNav()
                        ));

            return Task.CompletedTask;
        }
    }
}
