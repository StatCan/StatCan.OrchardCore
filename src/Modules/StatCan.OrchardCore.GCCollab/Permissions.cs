using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace StatCan.OrchardCore.GCCollab
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageGCCollabAuthentication
            = new Permission(nameof(ManageGCCollabAuthentication), "Manage GCCollab Authentication settings");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageGCCollabAuthentication }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            yield return new PermissionStereotype
            {
                Name = "Administrator",
                Permissions = new[]
                {
                    ManageGCCollabAuthentication
                }
            };
        }
    }
}
