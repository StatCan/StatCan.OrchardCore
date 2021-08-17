using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Schedule)]
    public class ScheduleMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public ScheduleMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {

            _contentDefinitionManager.CreateBasicWidget("ScheduleList");
            _contentDefinitionManager.AlterTypeDefinition("ScheduleList", t => t
                .WithTitlePart("0")
                .WithFlow("1", new string[] { "ScheduleEvent" })
            );
            _contentDefinitionManager.AlterPartDefinition("ScheduleEvent", p => p
                .WithTextField("Time", "0")
                .WithTextField("Location", "1")
            );
            _contentDefinitionManager.AlterTypeDefinition("ScheduleEvent", t => t
                .Stereotype("Widget")
                .WithTitlePart("0", TitlePartOptions.EditableRequired)
                .WithPart("ScheduleEvent", p => p.WithPosition("1"))
                .WithMarkdownBody("2")
            );
            return 1;
        }
    }
}
