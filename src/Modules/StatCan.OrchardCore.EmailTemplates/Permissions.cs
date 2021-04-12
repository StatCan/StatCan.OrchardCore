using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace StatCan.OrchardCore.EmailTemplates
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageEmailTemplates = new Permission("ManageEmailTemplates", "Manage email templates");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageEmailTemplates }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageEmailTemplates }
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { ManageEmailTemplates }
                }
            };
        }
    }
}
