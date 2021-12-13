using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Shell.Scope;
using OrchardCore.Users.Models;
using System;
using YesSql;

namespace StatCan.OrchardCore.Candev.Indexes
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            CreateCandevItemsIndex();
            CreateCandevUsersIndex();
            CreateHackathonAvgScoresIndex();

            return 2;
        }

        private void CreateCandevItemsIndex()
        {
            SchemaBuilder.CreateMapIndexTable(typeof(CandevItemsIndex), table => table
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

            SchemaBuilder.AlterTable(nameof(CandevItemsIndex), table => table
                 .CreateIndex("IDX_CandevItemsIndex_ContentItemId", "ContentItemId", "Latest", "Published", "CreatedUtc")
             );

            SchemaBuilder.AlterTable(nameof(CandevItemsIndex), table => table
                .CreateIndex("IDXCandevItemsIndex_ContentItemVersionId", "ContentItemVersionId")
            );

            SchemaBuilder.AlterTable(nameof(CandevItemsIndex), table => table
                .CreateIndex("IDX_CandevItemsIndex_DisplayText", "DisplayText")
            );

            SchemaBuilder.AlterTable(nameof(CandevItemsIndex), table => table
                .CreateIndex("IDX_CandevItemsIndex_TeamContentItemId", "TeamContentItemId")
            );

            SchemaBuilder.AlterTable(nameof(CandevItemsIndex), table => table
                .CreateIndex("IDX_CandevItemsIndex_CaseLocalizationSet", "CaseLocalizationSet")
            );
        }

        private void CreateCandevUsersIndex()
        {
            SchemaBuilder.CreateMapIndexTable(typeof(CandevUsersIndex), table => table
                .Column<string>("UserId", c => c.WithLength(26))
                .Column<string>("UserName", c => c.WithLength(26))
                .Column<string>("Email", c => c.Nullable().WithLength(255))
                .Column<string>("Roles", c => c.Nullable().WithLength(255))
                .Column<string>("ContactEmail", c => c.Nullable().WithLength(255))
                .Column<string>("FirstName", c => c.WithLength(26))
                .Column<string>("LastName", c => c.WithLength(26))
                .Column<string>("Language", c => c.WithLength(4))
                .Column<string>("TeamContentItemId", c => c.WithLength(26))
                .Column<bool>("WillAttend")
                .Column<bool>("CheckIn"),
                null
            );

            ShellScope.AddDeferredTask(async scope => {
                var session = scope.ServiceProvider.GetRequiredService<ISession>();
                var users = await session.Query<User>().ListAsync();
                foreach (var user in users)
                {
                    session.Save(user);
                }
            });
        }

        private void CreateHackathonAvgScoresIndex()
        {
            SchemaBuilder.CreateReduceIndexTable(typeof(HackathonAvgScoresIndex), table => table
                .Column<string>("TeamContentItemId", c => c.WithLength(26))
                .Column<double>("Score")
                .Column<double>("Count")
                .Column<double>("Average"),
                null
            );
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable(nameof(CandevUsersIndex), table => table.AddColumn<bool>("WillAttend"));
            SchemaBuilder.AlterTable(nameof(CandevUsersIndex), table => table.AddColumn<bool>("CheckIn"));
            CreateHackathonAvgScoresIndex();

            return 2;
        }
    }
}
