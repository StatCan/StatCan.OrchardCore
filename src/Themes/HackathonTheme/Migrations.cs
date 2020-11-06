using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.Themes.HackathonTheme
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            HackathonThemeSettings();
            Tabs();
            ScheduleEvent();
            return 1;
        }

        private void HackathonThemeSettings()
        {
            _contentDefinitionManager.AlterTypeDefinition("HackathonThemeSettings", type => type
                .DisplayedAs("Hackathon Theme Settings")
                .Stereotype("CustomSettings")
                .WithPart("HackathonThemeSettings", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("HackathonThemeSettings", part => part
                .WithField("DisplayName", field => field
                    .OfType("TextField")
                    .WithDisplayName("DisplayName")
                    .WithPosition("0"))
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("1")
                )
            );
        }

        private void Tabs()
        {
            _contentDefinitionManager.CreateBasicWidget("Tabs");
            _contentDefinitionManager.AlterTypeDefinition("Tabs", t => t
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                )
                .WithFlow("1", new string[] { "Tab" })
            );
            _contentDefinitionManager.CreateBasicWidget("Tab");
            _contentDefinitionManager.AlterTypeDefinition("Tab", t => t
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() { Options = TitlePartOptions.EditableRequired })
                )
                .WithFlow("2")
            );
        }

        private void ScheduleEvent()
        {
            _contentDefinitionManager.CreateBasicWidget("ScheduleList");
            _contentDefinitionManager.AlterTypeDefinition("ScheduleList", t => t
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                )
                .WithFlow("1", new string[] { "ScheduleEvent" })
            );
            _contentDefinitionManager.AlterPartDefinition("ScheduleEvent", p => p
                .WithTextField("Time", "0")
                .WithTextField("Location", "1")
            );
            _contentDefinitionManager.AlterTypeDefinition("ScheduleEvent", t => t
                .Stereotype("Widget")
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() { Options = TitlePartOptions.EditableRequired })
                )
                .WithPart("ScheduleEvent", p => p.WithPosition("1"))
                .WithMarkdownBody("2")
            );
        }

        private void ContentMenuItem()
        {
            _contentDefinitionManager.AlterPartDefinition("ContentMenuItem", part => part
                .WithField("IconName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon Name")
                    .WithPosition("0")
                )
            );
        }
    }
}