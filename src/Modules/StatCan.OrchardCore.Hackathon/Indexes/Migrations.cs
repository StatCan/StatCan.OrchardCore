using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using System;

namespace StatCan.OrchardCore.Hackathon.Indexes
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable(typeof(HackathonItemsIndex), table => table
                .Column<string>("ContentItemId", c => c.WithLength(26))
                .Column<string>("ContentItemVersionId", c => c.WithLength(26))
                .Column<string>("LocalizationSet", c => c.WithLength(26))
                .Column<string>("Culture", c => c.WithLength(8))
                .Column<bool>("Latest")
                .Column<bool>("Published")
                .Column<string>("ContentType", column => column.WithLength(ContentItemIndex.MaxContentTypeSize))
                .Column<DateTime>("ModifiedUtc", column => column.Nullable())
                .Column<DateTime>("PublishedUtc", column => column.Nullable())
                .Column<DateTime>("CreatedUtc", column => column.Nullable())
                .Column<string>("Owner", column => column.Nullable().WithLength(ContentItemIndex.MaxOwnerSize))
                .Column<string>("Author", column => column.Nullable().WithLength(ContentItemIndex.MaxAuthorSize))
                .Column<string>("DisplayText", column => column.Nullable().WithLength(ContentItemIndex.MaxDisplayTextSize))
                .Column<string>("Email", c => c.Nullable().WithLength(255))
                .Column<string>("TeamContentItemId", c => c.Nullable().WithLength(26))
                .Column<string>("CaseLocalizationSet", c => c.Nullable().WithLength(26)),
                nameof(HackathonItemsIndex)
            );

            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => table
                 .CreateIndex("IDX_HackathonItemsIndex_ContentItemId", "ContentItemId", "Latest", "Published", "CreatedUtc")
             );

            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => table
                .CreateIndex("IDX_HackathonItemsIndex_ContentItemVersionId", "ContentItemVersionId")
            );

            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => table
                .CreateIndex("IDX_HackathonItemsIndex_DisplayText", "DisplayText")
            );
            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => table
                .CreateIndex("IDX_HackathonItemsIndex_TeamContentItemId", "TeamContentItemId")
            );
            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => table
                .CreateIndex("IDX_HackathonItemsIndex_CaseLocalizationSet", "CaseLocalizationSet")
            );

            return 6;
        }

        public int UpdateFrom3()
        {
            SchemaBuilder.AlterTable(nameof(HackathonItemsIndex), table => {
                table.AddColumn<string>("LocalizationSet", c => c.WithLength(26));
                table.AddColumn<string>("Culture", c => c.WithLength(8));
            });
            return 4;
        }
    }
}
