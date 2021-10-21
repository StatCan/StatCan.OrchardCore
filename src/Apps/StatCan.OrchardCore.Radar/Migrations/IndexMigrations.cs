using OrchardCore.Data.Migration;
using YesSql.Sql;
using StatCan.OrchardCore.Radar.Indexing;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<RadarEntityPartIndex>(table => table
                .Column<string>(nameof(RadarEntityPartIndex.ContentItemId), column => column.WithLength(26))
                .Column<string>(nameof(RadarEntityPartIndex.ContentType))
                .Column<bool>(nameof(RadarEntityPartIndex.Published))
            );

            return 1;
        }
    }
}
