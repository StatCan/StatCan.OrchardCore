using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageSaaSConfiguration
            = new Permission(nameof(ManageSaaSConfiguration), "Manage SaaS configuration");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                ManageSaaSConfiguration,
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
                    ManageSaaSConfiguration,
                }
            };
        }
    }
}
