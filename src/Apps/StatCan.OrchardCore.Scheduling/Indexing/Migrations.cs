using System;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace StatCan.OrchardCore.Scheduling.Indexing
{
    public class IndexMigrations : DataMigration
    {
        public int Create()
        {
            SchemaBuilder.CreateMapIndexTable<AppointmentIndex>(table => table
                .Column<string>("ContentItemId", column => column.WithLength(26))
                .Column<string>("CalendarTermContentItemId", column => column.WithLength(26))
                .Column<DateTime>("StartDate", column => column.Nullable())
                .Column<TimeSpan>("StartTime", column => column.Nullable())
                .Column<int>("StartDay", column => column.Nullable())
                .Column<int>("StartMonth", column => column.Nullable())
                .Column<int>("StartYear", column => column.Nullable())
                .Column<DateTime>("EndDate", column => column.Nullable())
                .Column<TimeSpan>("EndTime", column => column.Nullable())
                .Column<int>("EndDay", column => column.Nullable())
                .Column<int>("EndMonth", column => column.Nullable())
                .Column<int>("EndYear", column => column.Nullable())
                .Column<string>("Status", column => column.WithLength(72))
            );

            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .CreateIndex("IDX_AptIndex_DocumentId",
                    "DocumentId",
                    "ContentItemId",
                    "CalendarTermContentItemId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .CreateIndex("IDX_AptIndex_DocumentId_CalendarId",
                    "DocumentId",
                    "CalendarTermContentItemId")
            );

            SchemaBuilder.CreateMapIndexTable<AppointmentLinkedIndex>(table => table
                .Column<string>("ContentItemId", column => column.WithLength(26))
                .Column<string>("CalendarTermContentItemId", column => column.WithLength(26))
                .Column<string>("LinkedContentItemId", column => column.WithLength(26))
            );

            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .CreateIndex("IDX_AptLinkedIndex_DocumentId",
                    "DocumentId",
                    "ContentItemId",
                    "CalendarTermContentItemId")
            );

            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .CreateIndex("IDX_AptLinkedIndex_DocumentId_LinkedId",
                    "DocumentId",
                    "LinkedContentItemId",
                    "CalendarTermContentItemId")
            );
            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .RenameColumn("LocationTermContentItemId", "CalendarTermContentItemId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .DropIndex("IDX_AptIndex_DocumentId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .CreateIndex("IDX_AptIndex_DocumentId",
                    "DocumentId",
                    "ContentItemId",
                    "CalendarTermContentItemId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .DropIndex("IDX_AptIndex_DocumentId_CalendarId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentIndex>(table => table
                .CreateIndex("IDX_AptIndex_DocumentId_CalendarId",
                    "DocumentId",
                    "CalendarTermContentItemId")
            );

            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                 .RenameColumn("LocationTermContentItemId", "CalendarTermContentItemId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .DropIndex("IDX_AptLinkedIndex_DocumentId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .CreateIndex("IDX_AptLinkedIndex_DocumentId",
                    "DocumentId",
                    "ContentItemId",
                    "CalendarTermContentItemId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .DropIndex("IDX_AptLinkedIndex_DocumentId_LinkedId")
            );
            SchemaBuilder.AlterIndexTable<AppointmentLinkedIndex>(table => table
                .CreateIndex("IDX_AptLinkedIndex_DocumentId_LinkedId",
                    "DocumentId",
                    "LinkedContentItemId",
                    "CalendarTermContentItemId")
            );

            return 2;
        }
    }
}
