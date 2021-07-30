using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Timeline)]
    public class TimelineMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public TimelineMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VTimeline();
            VTimelineItem();

            return 1;
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
    }
}
