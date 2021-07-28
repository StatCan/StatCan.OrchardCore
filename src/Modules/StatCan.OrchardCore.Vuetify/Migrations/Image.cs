using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Image)]
    public class ImageMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public ImageMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VImg();

            return 1;
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
    }
}
