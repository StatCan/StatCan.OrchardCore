using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Environment.Shell.Descriptor.Models;
using OrchardCore.Modules;
using OrchardCore.Navigation;

namespace StatCan.OrchardCore.GCCollab
{
    [Feature(GCCollabConstants.Features.GCCollabAuthentication)]
    public class AdminMenuGCCollabLogin : INavigationProvider
    {
        private readonly ShellDescriptor _shellDescriptor;
        private readonly IStringLocalizer S;

        public AdminMenuGCCollabLogin(
            IStringLocalizer<AdminMenuGCCollabLogin> localizer,
            ShellDescriptor shellDescriptor)
        {
            S = localizer;
            _shellDescriptor = shellDescriptor;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                builder.Add(S["Security"], security => security
                        .Add(S["Authentication"], authentication => authentication
                        .Add(S["GCCollab"], "14", settings => settings
                        .AddClass("gccollab").Id("gccollab")
                            .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = GCCollabConstants.Features.GCCollabAuthentication })
                            .Permission(Permissions.ManageGCCollabAuthentication)
                            .LocalNav())
                    ));
            }
            return Task.CompletedTask;
        }
    }
}
