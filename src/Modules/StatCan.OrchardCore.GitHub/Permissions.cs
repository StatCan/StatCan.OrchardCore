using OrchardCore.Security.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.GitHub
{
    public class Permissions: IPermissionProvider
    {
        public static readonly Permission ManageGitHubApiSettings
        = new Permission(nameof(ManageGitHubApiSettings), "Manage GitHub Api settings");
        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageGitHubApiSettings }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            yield return new PermissionStereotype
            {
                Name = "Administrator",
                Permissions = new[]
                {
                    ManageGitHubApiSettings
                }
            };
        }
    }
}
