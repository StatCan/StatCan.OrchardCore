using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Navigation;
using YesSql;

namespace StatCan.OrchardCore.Candev
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ISession _session;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer, ISession session, IContentManager contentManager, IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _contentManager = contentManager;
            _session = session;
            T = localizer;
        }

        public IStringLocalizer T { get; set; }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            // We want to add our menus to the "admin" menu only.
            if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(T["Admin Panel"], "-100", rootView =>
                {
                    rootView.Action("Index", "Admin", new { area = "StatCan.OrchardCore.Candev" });
                });

            return Task.CompletedTask;
        }
    }
}
