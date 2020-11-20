using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using OrchardCore.ContentFields.Settings;

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
            VContainer();
            VRow();
            VCol();
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
                .WithTextField("DisplayName", "0")
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
                .WithTitlePart("0")
                .WithFlow("1", new string[] { "Tab" })
            );
            _contentDefinitionManager.CreateBasicWidget("Tab");
            _contentDefinitionManager.AlterTypeDefinition("Tab", t => t
                .WithTitlePart("0", TitlePartOptions.EditableRequired)
                .WithFlow("2")
            );
        }

        private void ScheduleEvent()
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
        }

        private void MenuItems()
        {
            _contentDefinitionManager.AlterPartDefinition("ContentMenuItem", part => part
                .WithTextField("IconName","Icon Name" , "0")
            );
            _contentDefinitionManager.AlterPartDefinition("LinkMenuItem", part => part
                .WithTextField("IconName", "Icon Name", "0")
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
                .WithFlow("1")
            );

            var colsSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                DefaultValue = "12",
                Options = new ListValueOption[] {
                                        new ListValueOption(){Name = "Auto", Value = "auto"},
                                        new ListValueOption(){Name = "1", Value = "1"},
                                        new ListValueOption(){Name = "2", Value = "2"},
                                        new ListValueOption(){Name = "3", Value = "3"},
                                        new ListValueOption(){Name = "4", Value = "4"},
                                        new ListValueOption(){Name = "5", Value = "5"},
                                        new ListValueOption(){Name = "6", Value = "6"},
                                        new ListValueOption(){Name = "7", Value = "7"},
                                        new ListValueOption(){Name = "8", Value = "8"},
                                        new ListValueOption(){Name = "9", Value = "9"},
                                        new ListValueOption(){Name = "10", Value = "10"},
                                        new ListValueOption(){Name = "11", Value = "11"},
                                        new ListValueOption(){Name = "12", Value = "12"},
                                    }
            };

            var offsetSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,

                Options = new ListValueOption[] {
                    new ListValueOption(){Name = "None", Value = ""},
                    new ListValueOption(){Name = "1", Value = "1"},
                    new ListValueOption(){Name = "2", Value = "2"},
                    new ListValueOption(){Name = "3", Value = "3"},
                    new ListValueOption(){Name = "4", Value = "4"},
                    new ListValueOption(){Name = "5", Value = "5"},
                    new ListValueOption(){Name = "6", Value = "6"},
                    new ListValueOption(){Name = "7", Value = "7"},
                    new ListValueOption(){Name = "8", Value = "8"},
                    new ListValueOption(){Name = "9", Value = "9"},
                    new ListValueOption(){Name = "10", Value = "10"},
                    new ListValueOption(){Name = "11", Value = "11"},
                    new ListValueOption(){Name = "12", Value = "12"},
                }
            };

            _contentDefinitionManager.AlterPartDefinition("VCol", part => part
                .WithTextFieldPredefinedList("AlignSelf", "Align Self", "0", new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = EditorOption.Dropdown,
                        Options = new ListValueOption[] {
                                        new ListValueOption(){Name = "Default", Value = ""},
                                        new ListValueOption(){Name = "Start", Value = "start"},
                                        new ListValueOption(){Name = "Center", Value = "center"},
                                        new ListValueOption(){Name = "End", Value = "end"},
                                        new ListValueOption(){Name = "Auto", Value = "auto"},
                                        new ListValueOption(){Name = "Baseline", Value = "baseline"},
                                        new ListValueOption(){Name = "Stretch", Value = "stretch"}
                                    }
                    }
                )
                .WithTextFieldPredefinedList("Cols", "Cols Xs", "1", colsSettings)
                .WithTextFieldPredefinedList("ColsSm", "Cols Sm", "2", colsSettings)
                .WithTextFieldPredefinedList("ColsMd", "Cols Md", "3", colsSettings)
                .WithTextFieldPredefinedList("ColsLg", "Cols Lg", "4", colsSettings)
                .WithTextFieldPredefinedList("ColsXl", "Cols Xl", "5", colsSettings)
                .WithTextFieldPredefinedList("Offset", "Offset Xs", "6", offsetSettings)
                .WithTextFieldPredefinedList("OffsetSm", "Offset Sm", "7", offsetSettings)
                .WithTextFieldPredefinedList("OffsetMd", "Offset Md", "8", offsetSettings)
                .WithTextFieldPredefinedList("OffsetLg", "Offset Lg", "9", offsetSettings)
                .WithTextFieldPredefinedList("OffsetXl", "Offset Xl", "10", offsetSettings)
                .WithNumericField("Order", "Order Xs", "11")
                .WithNumericField("OrderSm", "Order Sm", "12")
                .WithNumericField("OrderMd", "Order Md", "13")
                .WithNumericField("OrderLg", "Order Lg", "14")
                .WithNumericField("OrderXl", "Order Xl", "15")
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
                .WithFlow("1", new[] { "VCol" })
            );
            var jutifySettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                Options = new ListValueOption[] {
                        new ListValueOption(){Name = "Default", Value = ""},
                        new ListValueOption(){Name = "Start", Value = "start"},
                        new ListValueOption(){Name = "Center", Value = "center"},
                        new ListValueOption(){Name = "End", Value = "end"},
                        new ListValueOption(){Name = "Between", Value = "space-between"},
                        new ListValueOption(){Name = "Around", Value = "space-around"},
                    }
            };

            var alignContentSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                Options = new ListValueOption[] {
                        new ListValueOption(){Name = "Default", Value = ""},
                        new ListValueOption(){Name = "Start", Value = "start"},
                        new ListValueOption(){Name = "Center", Value = "center"},
                        new ListValueOption(){Name = "End", Value = "end"},
                        new ListValueOption(){Name = "Between", Value = "space-between"},
                        new ListValueOption(){Name = "Around", Value = "space-around"},
                        new ListValueOption(){Name = "Stretch", Value = "stretch"},
                    }
            };

            var alignItemsSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                Options = new ListValueOption[] {
                        new ListValueOption(){Name = "Default", Value = ""},
                        new ListValueOption(){Name = "Start", Value = "start"},
                        new ListValueOption(){Name = "Center", Value = "center"},
                        new ListValueOption(){Name = "End", Value = "end"},
                        new ListValueOption(){Name = "Baseline", Value = "baseline"},
                        new ListValueOption(){Name = "Stretch", Value = "stretch"}
                    }
            };

            _contentDefinitionManager.AlterPartDefinition("VRow", part => part
                .WithMultiValueTextField("Props", "Props", "0", new ListValueOption[] {
                        new ListValueOption() {Name="No Gutters", Value="no-gutters"},
                        new ListValueOption() {Name="Dense", Value="dense"}
                    }
                )
                .WithTextFieldPredefinedList("Justify", "Justify Xs", "1", jutifySettings)
                .WithTextFieldPredefinedList("JustifySm", "Justify Sm", "2", jutifySettings)
                .WithTextFieldPredefinedList("JustifyLg", "Justify Lg", "3", jutifySettings)
                .WithTextFieldPredefinedList("JustifyMd", "Justify Md", "4", jutifySettings)
                .WithTextFieldPredefinedList("JustifyXl", "Justify Xl", "5", jutifySettings)
                .WithTextFieldPredefinedList("Align", "Align Xs", "6", alignItemsSettings)
                .WithTextFieldPredefinedList("AlignSm", "Align Sm", "7", alignItemsSettings)
                .WithTextFieldPredefinedList("AlignLg", "Align Lg", "8", alignItemsSettings)
                .WithTextFieldPredefinedList("AlignMd", "Align Md", "9", alignItemsSettings)
                .WithTextFieldPredefinedList("AlignXl", "Align Xl", "10", alignItemsSettings)
                .WithTextFieldPredefinedList("AlignContent", "Align Content Xs", "11", alignContentSettings)
                .WithTextFieldPredefinedList("AlignContentSm", "Align Content Sm", "12", alignContentSettings)
                .WithTextFieldPredefinedList("AlignContentLg", "Align Content Lg", "13", alignContentSettings)
                .WithTextFieldPredefinedList("AlignContentMd", "Align Content Md", "14", alignContentSettings)
                .WithTextFieldPredefinedList("AlignContentXl", "Align Content Xl", "15", alignContentSettings)

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
                .WithFlow("1", new[] { "VRow" })
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .WithMultiValueTextField("Props", "Props", "0",
                    new ListValueOption[] {
                         new ListValueOption() { Name="Fluid", Value="fluid"}
                    }
                )
            );
        }
    }
}
