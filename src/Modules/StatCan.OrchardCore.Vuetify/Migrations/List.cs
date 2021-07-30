using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Modules;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.List)]
    public class ListMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public ListMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VListItem();
            VList();
            VSubheader();

            return 1;
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
    }
}
