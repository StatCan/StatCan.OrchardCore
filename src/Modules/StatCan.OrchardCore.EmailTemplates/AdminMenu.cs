using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.EmailTemplates
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
                .Add(S["Design"], design => design
                    .Add(S["EmailTemplates"], "EmailTemplates", import => import
                        .Action("Index", "EmailTemplate", new { area = "StatCan.OrchardCore.EmailTemplates" })
                        .Permission(Permissions.ManageEmailTemplates)
                        .LocalNav()
                    )
                );

            return Task.CompletedTask;
        }
    }
}
