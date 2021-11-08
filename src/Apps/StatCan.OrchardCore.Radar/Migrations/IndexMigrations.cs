using System;
using YesSql.Sql;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.Radar.Indexes;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<RadarFormPartIndex>(table => table
                .Column<string>(nameof(RadarFormPartIndex.DisplayText))
                .Column<string>(nameof(RadarFormPartIndex.ContentItemId), column => column.WithLength(26))
                .Column<string>(nameof(RadarFormPartIndex.ContentType))
                .Column<bool>(nameof(RadarFormPartIndex.Published))
                .Column<DateTime>(nameof(RadarFormPartIndex.PublishedUtc))
            );

            return 1;
        }
    }
}
