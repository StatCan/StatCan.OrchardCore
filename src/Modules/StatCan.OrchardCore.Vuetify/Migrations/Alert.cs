using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Alert)]
    public class AlertMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public AlertMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VAlert();

            return 1;
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
    }
}
