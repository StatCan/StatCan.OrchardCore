using System;
using OrchardCore.Data.Migration;
using YesSql.Sql;
using StatCan.OrchardCore.Radar.Indexing;

namespace StatCan.OrchardCore.Radar.Migrations
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
            );

            return 1;
        }
    }
}
