using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace StatCan.OrchardCore.Matomo
{
    public class MatomoPermissions : IPermissionProvider
    {
        public static readonly Permission ManageMatomoSettings
            = new Permission(nameof(ManageMatomoSettings), "Manage Matomo settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                    ManageMatomoSettings
                }
            .AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            yield return new PermissionStereotype
            {
                Name = "Administrator",
                Permissions = new[]
                {
                        ManageMatomoSettings
                    }
            };
        }
    }
}
