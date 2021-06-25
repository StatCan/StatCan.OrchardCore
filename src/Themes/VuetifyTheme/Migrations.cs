using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Media.Settings;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings;
using OrchardCore.ContentFields.Fields;

namespace StatCan.Themes.VuetifyTheme
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
            VuetifyThemeSettings();
            Tabs();
            MenuItems();
            VContainer();
            VRow();
            VCol();
            VAlert();
            VCard();
            VExpansionPanel();
            VExpansionPanels();
            VImg();
            VTimeline();
            VTimelineItem();
            VContainerRow();
            VAppBar();
            VNavigationDrawer();
            AuthContentMenuItem();
            CompatibilityBanner();
            VFooter();
            UpdateToMultiTextField();
            VSubheader();
            VListItem();
            VList();
            VDivider();
            ScheduleEvent();
            TaxonomyMenuItem();
            return 5;
        }

        public int UpdateFrom1() {
            VSubheader();
            VListItem();
            VList();
            VDivider();
            return 2;
        }

        public int UpdateFrom2()
        {
            ScheduleEvent();
            return 3;
        }
        public int UpdateFrom3()
        {
            TaxonomyMenuItem();
            return 4;
        }
        public int UpdateFrom4()
        {
            VuetifyThemeSettings();
            return 5;
        }

        #region Private methods

        private void AuthContentMenuItem()
        {
            _contentDefinitionManager.AlterTypeDefinition("AuthContentMenuItem", type => type
                .DisplayedAs("Authenticated Content Menu Item")
                .Stereotype("MenuItem")
                .WithTitlePart("0")
                .WithPart("AuthContentMenuItem", part => part
                    .WithPosition("1")
                )
                .WithContentPermission("2")
            );

            _contentDefinitionManager.AlterPartDefinition("AuthContentMenuItem", part => part
                .WithTextField("IconName","Icon Name" , "0")
                .WithField("SelectedContentItem", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Selected Content Item")
                    .WithPosition("1")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        Required = true,
                        DisplayAllContentTypes = true
                    })
                )
            );
        }

        private void VuetifyThemeSettings()
        {
            _contentDefinitionManager.AlterTypeDefinition("VuetifyThemeSettings", type => type
                .DisplayedAs("Vuetify Theme Settings")
                .Stereotype("CustomSettings")
                .WithPart("VuetifyThemeSettings", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VuetifyThemeSettings", part => part
                .WithTextField("DisplayName", "0")
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("1")
                )
             .WithField("DisplayMode", field => field
                .OfType("TextField")
                .WithDisplayName("Display Mode")
                .WithEditor("PredefinedList")
                .WithPosition("2")
                .WithSettings(
                    new TextFieldPredefinedListEditorSettings
                        {
                            Options = new ListValueOption[] {
                                new ListValueOption(){Name = "Light Mode", Value= "light"},
                                new ListValueOption(){Name = "Dark Mode", Value= "dark"},
                                new ListValueOption(){Name = "Picker", Value= "picker"},
                        },
                }))

                .WithField("ThemeOptions", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Template")
                    .WithSettings(new TextFieldSettings() { Hint = "The Vuetify 'themes' object that defines the colors of both lite and dark theme. See https://vuetifyjs.com/en/features/theme/" })
                    .WithPosition("3")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"json\"}"
                        })
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
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "No Gutters", Value = "no-gutters"}
                        },
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
                .WithFlow("1", new[] { "VRow" })
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Fluid", Value = "fluid"},
                        }
                    })
                )
            );
        }

        private void VAlert()
        {
            _contentDefinitionManager.AlterTypeDefinition("VAlert", type => type
                .DisplayedAs("VAlert")
                .Stereotype("Widget")
                .WithPart("VAlert", part => part
                    .WithPosition("1")
                )
                .WithPart("FlowPart")
            );

            _contentDefinitionManager.AlterPartDefinition("VAlert", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Colored Border", Value = "colored-border"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Dismissible", Value = "dismissible"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Outlined", Value = "outlined"},
                            new MultiTextFieldValueOption() {Name = "Prominent", Value = "prominent"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"},
                            new MultiTextFieldValueOption() {Name = "Text", Value = "text"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                        },
                    })
                )
                .WithField("Border", field => field
                    .OfType("TextField")
                    .WithDisplayName("Border")
                    .WithEditor("PredefinedGroup")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Puts a border on the alert.",
                    })
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name = "Top",
                            Value = "top"
                        }, new ListValueOption() {
                            Name = "Right",
                            Value = "right"
                        }, new ListValueOption() {
                            Name = "Bottom",
                            Value = "bottom"
                        }, new ListValueOption() {
                            Name = "Left",
                            Value = "left"
                        } },
                    })
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithEditor("Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("Elevation", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Elevation")
                    .WithEditor("Slider")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Designates an elevation applied to the component between 0 and 24",
                        Minimum = 0,
                        Maximum = 24,
                        DefaultValue = "0",
                    })
                )
                .WithField("Icon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Designates a specific icon.",
                    })
                )
                .WithField("Type", field => field
                    .OfType("TextField")
                    .WithDisplayName("Type")
                    .WithEditor("PredefinedGroup")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Specify a success, info, warning or error alert. Uses the contextual color and has a pre-defined icon.",
                    })
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name = "Success",
                            Value = "success"
                        }, new ListValueOption() {
                            Name = "Info",
                            Value = "info"
                        }, new ListValueOption() {
                            Name = "Warning",
                            Value = "warning"
                        }, new ListValueOption() {
                            Name = "Error",
                            Value = "error"
                        } },
                    })
                )
            );
        }

        private void VCard()
        {
            _contentDefinitionManager.AlterTypeDefinition("VCard", type => type
                .DisplayedAs("VCard")
                .Stereotype("Widget")
                .WithPart("VCard", part => part
                    .WithPosition("1")
                )
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ Model.ContentItem.Content.VCard.Title.Text }}")
                .WithPart("FlowPart", part => part
                    .WithPosition("2")
                )
                .WithPart("Actions", part => part
                    .WithDisplayName("Actions")
                    .WithDescription("Used for placing actions for a card.")
                    .WithPosition("3")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { "ContentMenuItem", "LinkMenuItem", "Menu" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VCard", part => part
                .WithField("Title", field => field
                    .OfType("TextField")
                    .WithDisplayName("Title")
                    .WithPosition("0")
                )
                .WithField("Subtitle", field => field
                    .OfType("TextField")
                    .WithDisplayName("Subtitle")
                    .WithPosition("1")
                )
            );
        }

        private void VExpansionPanel()
        {
            _contentDefinitionManager.AlterTypeDefinition("VExpansionPanel", type => type
                .DisplayedAs("VExpansionPanel")
                .Stereotype("Widget")
                .WithPart("VExpansionPanel", part => part
                    .WithPosition("1")
                )
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ Model.ContentItem.Content.VExpansionPanel.Header.Text }}")
                .WithPart("FlowPart", part => part
                    .WithPosition("2")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VExpansionPanel", part => part
                .WithField("Header", field => field
                    .OfType("TextField")
                    .WithDisplayName("Header")
                    .WithPosition("0")
                )
            );
        }

        private void VExpansionPanels()
        {
            _contentDefinitionManager.AlterTypeDefinition("VExpansionPanels", type => type
                .DisplayedAs("VExpansionPanels")
                .Stereotype("Widget")
                .WithPart("VExpansionPanels", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VExpansionPanels", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Accordion", Value = "accordion"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Disabled", Value = "disabled"},
                            new MultiTextFieldValueOption() {Name = "Flat", Value = "flat"},
                            new MultiTextFieldValueOption() {Name = "Focusable", Value = "focusable"},
                            new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Mandatory", Value = "mandatory"},
                            new MultiTextFieldValueOption() {Name = "Multiple", Value = "multiple"},
                            new MultiTextFieldValueOption() {Name = "Popout", Value = "popout"},
                            new MultiTextFieldValueOption() {Name = "Read-Only", Value = "readonly"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                            },
                    })
                )
            );
        }

        private void VImg()
        {
            _contentDefinitionManager.AlterTypeDefinition("VImg", type => type
                .DisplayedAs("VImg")
                .Stereotype("Widget")
                .WithPart("VImg", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VImg", part => part
                .WithField("AlternateText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Alternate Text")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Alternate text for screen readers. Leave empty for decorative images.",
                    })
                )
                .WithField("AspectRatio", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Aspect Ratio")
                    .WithPosition("2")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Calculated as width/height, so for a 1920x1080px image this will be 1.7778. Will be calculated automatically if omitted.",
                        Scale = 4,
                    })
                )
                .WithField("Gradient", field => field
                    .OfType("TextField")
                    .WithDisplayName("Gradient")
                    .WithPosition("5")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Overlays a gradient onto the image. Only supports linear-gradient syntax.",
                    })
                )
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithPosition("6")
                )
                .WithField("Source", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Source")
                    .WithPosition("0")
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithPosition("7")
                )
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Contain", Value = "contain"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Eager", Value = "eager"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"}
                        },
                    })
                )
            );
        }

        private void VTimeline()
        {
            _contentDefinitionManager.AlterTypeDefinition("VTimeline", type => type
                .DisplayedAs("VTimeline")
                .Stereotype("Widget")
                .WithPart("VTimeline", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithSettings(new FlowPartSettings
                    {
                        ContainedContentTypes = new[] { "VTimelineItem" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VTimeline", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Align Top", Value = "align-top"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Reverse", Value = "reverse"}
                        },
                    })
                )
            );
        }

        private void VTimelineItem()
        {
            _contentDefinitionManager.AlterTypeDefinition("VTimelineItem", type => type
                .DisplayedAs("VTimelineItem")
                .Stereotype("Widget")
                .WithPart("VTimelineItem", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart")
            );

            _contentDefinitionManager.AlterPartDefinition("VTimelineItem", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Fill Dot", Value = "fill-dot"},
                            new MultiTextFieldValueOption() {Name = "Hide Dot", Value = "hide-dot"},
                            new MultiTextFieldValueOption() {Name = "Large", Value = "large"},
                            new MultiTextFieldValueOption() {Name = "Left", Value = "left"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Right", Value = "right"},
                            new MultiTextFieldValueOption() {Name = "Small", Value = "small"}
                        },
                    })
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("Icon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Specify icon for dot container. https://materialdesignicons.com",
                    })
                )
                .WithField("IconColor", field => field
                    .OfType("TextField")
                    .WithDisplayName("Icon Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
            );
        }

        private void VContainerRow()
        {
            _contentDefinitionManager.AlterTypeDefinition("VContainerRow", type => type
                .DisplayedAs("VContainerRow")
                .Stereotype("Widget")
                .WithPart("VContainerRow", part => part
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

            _contentDefinitionManager.AlterPartDefinition("VContainerRow", part => part
                .WithField("Layout", field => field
                    .OfType("TextField")
                    .WithDisplayName("Layout")
                    .WithEditor("PredefinedGroup")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(2.04358,0,0,0.978801,0.178752,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "12"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.00094,0,0,0.978801,0.59795,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.00094,0,0,0.978801,50.5979,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "6-6"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.667293,0,0,0.978801,0.731966,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.667293,0,0,0.978801,33.732,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.667293,0,0,0.978801,66.732,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "4-4-4"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.458764,0,0,0.978801,0.815727,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.479617,0,0,0.978801,23.8074,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.521323,0,0,0.978801,47.7906,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.521323,0,0,0.978801,73.7906,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-3-3-3"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.834116,0,0,0.978801,0.664958,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.18862,0,0,0.978801,41.5226,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "5-7"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.18862,0,0,0.978801,0.522565,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.834116,0,0,0.978801,58.665,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "7-5"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.563029,0,0,0.978801,0.773847,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.4597,0,0,0.978801,28.4137,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-9"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.4597,0,0,0.978801,0.413676,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.563029,0,0,0.978801,71.7738,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "9-3"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.563029,0,0,0.978801,0.773847,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.875822,0,0,0.978801,28.6482,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.563029,0,0,0.978801,71.7738,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-6-3"
                        } },
                        DefaultValue = "6-6",
                    })
                )
            );
        }

        private void VAppBar()
        {
            _contentDefinitionManager.AlterTypeDefinition("VAppBar", type => type
                .DisplayedAs("VAppBar")
                .Stereotype("Widget")
                .WithPart("VAppBar", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VAppBar", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Absolute", Value = "absolute"},
                            new MultiTextFieldValueOption() {Name = "Bottom", Value = "bottom"},
                            new MultiTextFieldValueOption() {Name = "Collapse", Value = "collapse"},
                            new MultiTextFieldValueOption() {Name = "Collapse On Scroll", Value = "collapse-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Elevate On Scroll", Value = "elevate-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Extended", Value = "extended"},
                            new MultiTextFieldValueOption() {Name = "Fade Image On Scroll", Value = "fade-img-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Fixed", Value = "fixed"},
                            new MultiTextFieldValueOption() {Name = "Flat", Value = "flat"},
                            new MultiTextFieldValueOption() {Name = "Floating", Value = "floating"},
                            new MultiTextFieldValueOption() {Name = "Hide On Scroll", Value = "hide-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Inverted Scroll", Value = "inverted-scroll"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Outlined", Value = "outlined"},
                            new MultiTextFieldValueOption() {Name = "Prominent", Value = "prominent"},
                            new MultiTextFieldValueOption() {Name = "Scroll Off Screen", Value = "scroll-off-screen"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"},
                            new MultiTextFieldValueOption() {Name = "Short", Value = "short"},
                            new MultiTextFieldValueOption() {Name = "Shrink On Scroll", Value = "shrink-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                        },
                    })
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("Elevation", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Elevation")
                    .WithEditor("Slider")
                    .WithPosition("3")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Designates an elevation applied to the component between 0 and 24.",
                        Minimum = 0,
                        Maximum = 24,
                    })
                )
                .WithField("ExtensionHeight", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Extension Height")
                    .WithPosition("4")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Specify an explicit height for the extension slot.",
                        Minimum = 0,
                        Maximum = 9999,
                        DefaultValue = "48",
                    })
                )
                .WithField("ScrollThreshold", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Scroll Threshold")
                    .WithPosition("5")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "The amount of scroll distance down before hide-on-scroll activates.",
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("BackgroundImage", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Background Image")
                    .WithPosition("1")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "Image source. See v-img for details.",
                        Multiple = false,
                    })
                )
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithPosition("6")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Sets the height for the component.",
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithPosition("7")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Sets the width for the component.",
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
            );
        }

        private void VNavigationDrawer()
        {
            _contentDefinitionManager.AlterTypeDefinition("VNavigationDrawer", type => type
                .DisplayedAs("VNavigationDrawer")
                .Stereotype("Widget")
                .WithPart("VNavigationDrawer", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VNavigationDrawer", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Absolute", Value = "absolute"},
                            new MultiTextFieldValueOption() {Name = "Bottom", Value = "bottom"},
                            new MultiTextFieldValueOption() {Name = "Clipped", Value = "clipped"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Disable Resize Watcher", Value = "disable-resize-watcher"},
                            new MultiTextFieldValueOption() {Name = "Disable Route Watcher", Value = "disable-route-watcher"},
                            new MultiTextFieldValueOption() {Name = "Expand on Hover", Value = "expand-on-hover"},
                            new MultiTextFieldValueOption() {Name = "Floating", Value = "floating"},
                            new MultiTextFieldValueOption() {Name = "Hide Overlay", Value = "hide-overlay"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Mini", Value = "mini-variant"},
                            new MultiTextFieldValueOption() {Name = "Permanent", Value = "permanent"},
                            new MultiTextFieldValueOption() {Name = "Right", Value = "right"},
                            new MultiTextFieldValueOption() {Name = "Stateless", Value = "stateless"},
                            new MultiTextFieldValueOption() {Name = "Temporary", Value = "temporary"},
                            new MultiTextFieldValueOption() {Name = "Touchless", Value = "touchless"}
                        },
                    })
                )
                .WithField("BackgroundImage", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Background Image")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "Specifies a v-img as the component’s background.",
                        Multiple = false,
                    })
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("MiniVariantWidth", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Mini Variant Width")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Designates the width assigned when the mini prop is turned on.",
                        Minimum = 0,
                        Maximum = 9999,
                        DefaultValue = "56",
                    })
                )
                .WithField("MobileBreakpoint", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Mobile Breakpoint")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Sets the designated mobile breakpoint for the component. This will apply alternate styles for mobile devices such as the temporary prop, or activate the bottom prop when the breakpoint value is met. Setting the value to 0 will disable this functionality.",
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
                .WithField("OverlayColor", field => field
                    .OfType("TextField")
                    .WithDisplayName("Overlay Color")
                )
                .WithField("OverlayOpacity", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Overlay Opacity")
                    .WithEditor("Slider")
                    .WithSettings(new NumericFieldSettings
                    {
                        Scale = 4,
                        Minimum = 0,
                        Maximum = 1,
                    })
                )
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Sets the height for the component.",
                        Minimum = 0,
                    })
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Sets the width for the component.",
                        Minimum = 0,
                        Maximum = 9999,
                    })
                )
            );
        }

        private void CompatibilityBanner()
        {
            _contentDefinitionManager.AlterTypeDefinition("CompatibilityBanner", type => type
                .DisplayedAs("CompatibilityBanner")
                .Stereotype("Widget")
                .WithPart("CompatibilityBanner", part => part
                    .WithPosition("0")
                )
            );
        }

        private void VFooter()
        {
            _contentDefinitionManager.AlterTypeDefinition("VFooter", type => type
                .DisplayedAs("VFooter")
                .Stereotype("Widget")
                .WithPart("VFooter", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VFooter", part => part
                .WithField("Height", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Height")
                    .WithPosition("1")
                )
                .WithField("Width", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Width")
                    .WithPosition("2")
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithPosition("0")
                )
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Absolute", Value = "absolute"},
                            new MultiTextFieldValueOption() {Name = "App", Value = "app"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Fixed", Value = "fixed"},
                            new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Outlined", Value = "outline"},
                            new MultiTextFieldValueOption() {Name = "Padless", Value = "padless"},
                            new MultiTextFieldValueOption() {Name = "Rounded", Value = "rounded"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                        },
                    })
                )
            );
        }

        private void UpdateToMultiTextField() {


            _contentDefinitionManager.AlterPartDefinition("VAppBar", part => part
                            .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VAppBar", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Absolute", Value = "absolute"},
                            new MultiTextFieldValueOption() {Name = "Bottom", Value = "bottom"},
                            new MultiTextFieldValueOption() {Name = "Collapse", Value = "collapse"},
                            new MultiTextFieldValueOption() {Name = "Collapse On Scroll", Value = "collapse-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Elevate On Scroll", Value = "elevate-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Extended", Value = "extended"},
                            new MultiTextFieldValueOption() {Name = "Fade Image On Scroll", Value = "fade-img-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Fixed", Value = "fixed"},
                            new MultiTextFieldValueOption() {Name = "Flat", Value = "flat"},
                            new MultiTextFieldValueOption() {Name = "Floating", Value = "floating"},
                            new MultiTextFieldValueOption() {Name = "Hide On Scroll", Value = "hide-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Inverted Scroll", Value = "inverted-scroll"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Outlined", Value = "outlined"},
                            new MultiTextFieldValueOption() {Name = "Prominent", Value = "prominent"},
                            new MultiTextFieldValueOption() {Name = "Scroll Off Screen", Value = "scroll-off-screen"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"},
                            new MultiTextFieldValueOption() {Name = "Short", Value = "short"},
                            new MultiTextFieldValueOption() {Name = "Shrink On Scroll", Value = "shrink-on-scroll"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                        },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Fluid", Value = "fluid"},
                        }
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VExpansionPanels", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VExpansionPanels", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Accordion", Value = "accordion"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Disabled", Value = "disabled"},
                            new MultiTextFieldValueOption() {Name = "Flat", Value = "flat"},
                            new MultiTextFieldValueOption() {Name = "Focusable", Value = "focusable"},
                            new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Mandatory", Value = "mandatory"},
                            new MultiTextFieldValueOption() {Name = "Multiple", Value = "multiple"},
                            new MultiTextFieldValueOption() {Name = "Popout", Value = "popout"},
                            new MultiTextFieldValueOption() {Name = "Read-Only", Value = "readonly"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"}
                            },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VImg", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VImg", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Contain", Value = "contain"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Eager", Value = "eager"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"}
                        },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VNavigationDrawer", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VNavigationDrawer", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Absolute", Value = "absolute"},
                            new MultiTextFieldValueOption() {Name = "Bottom", Value = "bottom"},
                            new MultiTextFieldValueOption() {Name = "Clipped", Value = "clipped"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Disable Resize Watcher", Value = "disable-resize-watcher"},
                            new MultiTextFieldValueOption() {Name = "Disable Route Watcher", Value = "disable-route-watcher"},
                            new MultiTextFieldValueOption() {Name = "Expand on Hover", Value = "expand-on-hover"},
                            new MultiTextFieldValueOption() {Name = "Floating", Value = "floating"},
                            new MultiTextFieldValueOption() {Name = "Hide Overlay", Value = "hide-overlay"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Mini", Value = "mini-variant"},
                            new MultiTextFieldValueOption() {Name = "Permanent", Value = "permanent"},
                            new MultiTextFieldValueOption() {Name = "Right", Value = "right"},
                            new MultiTextFieldValueOption() {Name = "Stateless", Value = "stateless"},
                            new MultiTextFieldValueOption() {Name = "Temporary", Value = "temporary"},
                            new MultiTextFieldValueOption() {Name = "Touchless", Value = "touchless"}
                        },
                    })
                )
             );

            _contentDefinitionManager.AlterPartDefinition("VRow", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VRow", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "No Gutters", Value = "no-gutters"}
                        },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VTimeline", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VTimeline", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Align Top", Value = "align-top"},
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Reverse", Value = "reverse"}
                        },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VTimelineItem", part => part
                .RemoveField("Props")
            );

            _contentDefinitionManager.AlterPartDefinition("VTimelineItem", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Fill Dot", Value = "fill-dot"},
                            new MultiTextFieldValueOption() {Name = "Hide Dot", Value = "hide-dot"},
                            new MultiTextFieldValueOption() {Name = "Large", Value = "large"},
                            new MultiTextFieldValueOption() {Name = "Left", Value = "left"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Right", Value = "right"},
                            new MultiTextFieldValueOption() {Name = "Small", Value = "small"}
                        },
                    })
                )
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

        private void CreateFip()
        {
            _contentDefinitionManager.AlterTypeDefinition("FIP", type => type
                .DisplayedAs("FIP")
                .Stereotype("Widget")
                .WithPart("FIP", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("FIP", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Rounded", Value = "rounded"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"}
                        },
                    })
                )
            );
        }

        private void VSubheader() {
            _contentDefinitionManager.AlterTypeDefinition("VSubheader", type => type
                .DisplayedAs("VSubheader")
                .WithPart("VSubheader", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VSubheader", part => part
                .WithField("Title", field => field
                    .OfType("TextField")
                    .WithDisplayName("Title")
                    .WithPosition("0")
                )
                .WithField("Props", field => field
                .OfType("MultiTextField")
                .WithDisplayName("Props")
                .WithEditor("Picker")
                .WithSettings(new MultiTextFieldSettings
                {
                    Options = new MultiTextFieldValueOption[] {
                        new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                        new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                        new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                    },
                })
                )
            );
        }
        private void VListItem() {
            _contentDefinitionManager.AlterTypeDefinition("VListItem", type => type
                .DisplayedAs("VListItem")
                .WithPart("VListItem", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VListItem", part => part
                .WithTextField("ItemTitle", "Item Title", "0")
                .WithTextField("IconName", "Icon Name", "1")
                .WithTextField("ItemSubTitle", "Item Subtitle", "2")
                .WithTextField("ItemText", "Item Text", "3")
                .WithTextField("ActiveClass", "Active Class", "4", new TextFieldSettings(){
                    Hint = "Configure the active CSS class applied when the link is active."
                })
                .WithTextField("Color", "Color", "5", new TextFieldSettings(){
                    Hint = "Applies specified color to the control when in an active state or input-value is true - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5))"
                })
                .WithTextField("ExactActiveClass", "Exact Active Class", "6", new TextFieldSettings(){
                    Hint = "Configure the active CSS class applied when the link is active with exact match."
                })
                .WithTextField("Href", "Href", "7", new TextFieldSettings(){
                    Hint = "Designates the component as anchor and applies the href attribute."
                })
                .WithTextField("InputValue", "Input Value", "8", new TextFieldSettings(){
                    Hint = "Controls the active state of the item. This is typically used to highlight the component"
                })
                .WithTextField("Target", "Target", "9", new TextFieldSettings(){
                    Hint = "Designates the target attribute. This should only be applied when using the href prop."
                })
                .WithTextField("To", "To", "10", new TextFieldSettings(){
                    Hint = "Denotes the target route of the link."
                })
            );
        }

        private void VList() {
            _contentDefinitionManager.AlterTypeDefinition("VList", type => type
                .DisplayedAs("VList")
                .Stereotype("Widget")
                .WithPart("VList", part => part
                    .WithPosition("0")
                )
                .WithPart("Items", "BagPart", part => part
                    .WithDisplayName("Items")
                    .WithDescription("Provides a collection behavior for your content item where you can place other content items.")
                    .WithPosition("1")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new[] { "VSubheader", "VListItem", "VDivider" },
                    })
                )
                );

            _contentDefinitionManager.AlterPartDefinition("VList", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "Disabled", Value = "disabled"},
                            new MultiTextFieldValueOption() {Name = "Expand", Value = "expand"},
                            new MultiTextFieldValueOption() {Name = "Flat", Value = "flat"},
                            new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                            new MultiTextFieldValueOption() {Name = "Nav", Value = "nav"},
                            new MultiTextFieldValueOption() {Name = "Outlined", Value = "outlined"},
                            new MultiTextFieldValueOption() {Name = "Rounded", Value = "rounded"},
                            new MultiTextFieldValueOption() {Name = "Shaped", Value = "shaped"},
                            new MultiTextFieldValueOption() {Name = "Subheader", Value = "subheader"},
                            new MultiTextFieldValueOption() {Name = "Tile", Value = "tile"},
                    }
                    })
                )
                .WithTextField("Color", "Color", "1", new TextFieldSettings(){
                    Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5))."
                })
                .WithTextField("Rounded", "Rounded", "2", new TextFieldSettings() {
                    Hint = "Designates the border-radius applied to the component."
                })
                .WithNumericField("Elevation", "3", new NumericFieldSettings() {
                    Hint = "Designates an elevation applied to the component between 0 and 24. ",
                    Minimum = 0,
                    Maximum = 24
                })
                .WithNumericField("Height", "4", new NumericFieldSettings() {
                    Hint = "Sets the height for the component.",
                })
                .WithNumericField("Width", "5", new NumericFieldSettings() {
                    Hint = "Sets the width for the component.",
                })
            );
        }
        private void VDivider() {
            _contentDefinitionManager.AlterTypeDefinition("VDivider", type => type
                .DisplayedAs("VDivider")
                .Stereotype("Widget")
                .WithPart("VDivider", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VDivider", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                        new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                        new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                        new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                        new MultiTextFieldValueOption() {Name = "Vertical", Value = "vertical"},
                    },
                    })
                )
            );
        }

        private void TaxonomyMenuItem(){
            _contentDefinitionManager.AlterTypeDefinition("TaxonomyMenuItem", type => type
                .DisplayedAs("Taxonomy Menu Item")
                .Stereotype("MenuItem")
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
                .WithPart("TaxonomyMenuItem", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("TaxonomyMenuItem", part => part
                .WithField("Taxonomy", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Taxonomy")
                    .WithPosition("0")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "Taxonomy" },
                    })
                )
                .WithTextField("IconName", "Icon Name", "1")
                .WithField("Expanded", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Expanded")
                    .WithPosition("2")
                )
            );
        }
        #endregion
    }
}
