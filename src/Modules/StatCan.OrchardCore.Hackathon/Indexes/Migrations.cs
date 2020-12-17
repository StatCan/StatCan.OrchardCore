using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Users.Models;
using System;
using YesSql;

namespace StatCan.OrchardCore.Hackathon.Indexes
{
    public class IndexMigrations : DataMigration
    {
        private readonly ISession _session;
        public IndexMigrations(ISession session)
        {
            _session = session;
        }

        public int Create()
        {
            CreateHackathonItemsIndex();
            CreateHackathonUsersIndex();

            return 1;
        }

        private void CreateHackathonItemsIndex()
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
                null
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
        }

        private async void CreateHackathonUsersIndex()
        {
            SchemaBuilder.CreateMapIndexTable(typeof(HackathonUsersIndex), table => table
                .Column<string>("UserId", c => c.WithLength(26))
                .Column<string>("UserName", c => c.WithLength(26))
                .Column<string>("Email", c => c.Nullable().WithLength(255))
                .Column<string>("FirstName", c => c.WithLength(26))
                .Column<string>("LastName", c => c.WithLength(26))
                .Column<string>("Language", c => c.WithLength(4))
                .Column<string>("TeamContentItemId", c => c.WithLength(26)),
                null
            );

            var users = await _session.Query<User>().ListAsync();
            foreach (var user in users)
            {
                _session.Save(user);
            }
        }
    }
}
