using System;
using YesSql.Sql;
using OrchardCore.Data.Migration;
using Etch.OrchardCore.ContentPermissions.Indexing;

namespace Etch.OrchardCore.ContentPermissions.Migrations
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<ContentPermissionsPartIndex>(table => table
                .Column<string>(nameof(ContentPermissionsPartIndex.ContentItemId), column => column.WithLength(26))
                .Column<string>(nameof(ContentPermissionsPartIndex.ContentType))
                .Column<bool>(nameof(ContentPermissionsPartIndex.Published))
                .Column<DateTime>(nameof(ContentPermissionsPartIndex.PublishedUtc))
                .Column<string>(nameof(ContentPermissionsPartIndex.Roles))
                .Column<bool>(nameof(ContentPermissionsPartIndex.Enabled))
            );

            return 1;
        }
    }
}
