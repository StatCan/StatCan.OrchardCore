using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.ExpansionPanel)]
    public class ExpansionPanelMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public ExpansionPanelMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VExpansionPanel();
            VExpansionPanels();

            return 1;
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

    }
}
