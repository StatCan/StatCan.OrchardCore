using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using OrchardCore.Flows.Models;
using ListValueOption = StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings.ListValueOption;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;


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
            MenuItems();
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

        private void MenuItems()
        {
            _contentDefinitionManager.AlterPartDefinition("ContentMenuItem", part => part
                .WithField("IconName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon Name")
                    .WithPosition("0")
                )
            );
            _contentDefinitionManager.AlterPartDefinition("LinkMenuItem", part => part
                .WithField("IconName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon Name")
                    .WithPosition("0")
                )
            );
        }

        private void VCol()
        {
            _contentDefinitionManager.AlterTypeDefinition("VCol", type => type
                .DisplayedAs("VCol")
                .Stereotype("Widget")
                .WithPart("VCol", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VCol", part => part
                .WithField("Props", field => field
                    .OfType("MultiValueTextField")
                    .WithDisplayName("Props")
                    .WithEditor("PredefinedList")
                    .WithPosition("0")
                    .WithSettings(new MultiValueTextFieldEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                    Name="Align Self Start",
                    Value="align-self=\"start\""
                    }, new ListValueOption() {
                    Name="Align Self Center",
                    Value="align-self=\"center\""
                    }, new ListValueOption() {
                    Name="Align Self End",
                    Value="align-self=\"end\""
                    }, new ListValueOption() {
                    Name="Align Self Auto",
                    Value="align-self=\"auto\""
                    }, new ListValueOption() {
                    Name="Align Self Baseline",
                    Value="align-self=\"baseline\""
                    }, new ListValueOption() {
                    Name="Align Self Stretch",
                    Value="align-self=\"stretch\""
                    }, new ListValueOption() {
                    Name="Col 12 (Full)",
                    Value="md=\"12\""
                    }, new ListValueOption() {
                    Name="Col 8 (3/4)",
                    Value="md=\"8\""
                    }, new ListValueOption() {
                    Name="Col 6 (1/2)",
                    Value="md=\"6\""
                    }, new ListValueOption() {
                    Name="Col 4 (1/4)",
                    Value="md=\"4\""
                    } }
                    })
                )
            );


        }
        private void VRow()
        {
            _contentDefinitionManager.AlterTypeDefinition("VRow", type => type
                .DisplayedAs("VRow")
                .Stereotype("Widget")
                .WithPart("VRow", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                    .WithSettings(new FlowPartSettings
                    {
                        ContainedContentTypes = new[] { "VCol" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VRow", part => part
                .WithField("Props", field => field
                    .OfType("MultiValueTextField")
                    .WithDisplayName("Props")
                    .WithEditor("PredefinedList")
                    .WithPosition("0")
                    .WithSettings(new MultiValueTextFieldEditorSettings
                    {
                        Options = new ListValueOption[] {
                            new ListValueOption() {
                        Name="No Gutters",
                        Value="no-gutters"
                        }, new ListValueOption() {
                        Name="Justify Start",
                        Value="justify=\"start\""
                        }, new ListValueOption() {
                        Name="Justify Center",
                        Value="justify=\"center\""
                        }, new ListValueOption() {
                        Name="Justify End",
                        Value="justify=\"end\""
                        }, new ListValueOption() {
                        Name="Justify Between",
                        Value="justify=\"space-between\""
                        }, new ListValueOption() {
                        Name="Justify Around",
                        Value="justify=\"space-around\""
                        } }
                    })
                )
            );


        }
        private void VContainer()
        {
            _contentDefinitionManager.AlterTypeDefinition("VContainer", type => type
                .DisplayedAs("VContainer")
                .Stereotype("Widget")
                .WithPart("VContainer", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                    .WithSettings(new FlowPartSettings
                    {
                        ContainedContentTypes = new[] { "VRow" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .WithField("Props", field => field
                    .OfType("MultiValueTextField")
                    .WithDisplayName("Props")
                    .WithEditor("PredefinedList")
                    .WithSettings(new MultiValueTextFieldEditorSettings
                    {
                        Options = new ListValueOption[] {
                            new ListValueOption() {
                            Name="Fluid",
                            Value="fluid"
                        }
                    }
                    })
                )
            );
        }
    }
}

