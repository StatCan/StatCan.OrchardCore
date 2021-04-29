using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security.Permissions;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageGraphQLQueries = new Permission("ManageGraphQLQueries", "Manage GraphQL Queries");

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[]
            {
                ManageGraphQLQueries
            }
            .AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = "Administrator",
                    Permissions = new[] { ManageGraphQLQueries }
                },
                new PermissionStereotype
                {
                    Name = "Editor",
                    Permissions = new[] { ManageGraphQLQueries }
                }
            };
        }
    }
}
