using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using OrchardCore.Media.Settings;

namespace StatCan.OrchardCore.Persona
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
            CreatePersona();
            return 1;
        }

        private void CreatePersona() {
            _contentDefinitionManager.AlterTypeDefinition("Persona", type => type
                .DisplayedAs("Persona")
                .Stereotype("Widget")
                .WithPart("Persona", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Persona", part => part
                .WithField("Data", field => field
                    .OfType("TextField")
                    .WithDisplayName("Data")
                    .WithEditor("CodeMirror")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Enter JSON for Persona here.",
                        Required = true,
                    })
                )
                .WithField("PhotoPicker", field => field
                    .OfType("MediaField")
                    .WithDisplayName("PhotoPicker")
                    .WithPosition("2")
                    .WithSettings(new MediaFieldSettings
                    {
                        Hint = "Select the photo to use for the persona.",
                        Required = false,
                        Multiple = false,
                        AllowMediaText = false,
                        AllowAnchors = true,
                    })
                )
                .WithField("Colour", field => field
                    .OfType("TextField")
                    .WithDisplayName("Colour")
                    .WithEditor("Color")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Choose the colour that matches the site's theme.",
                        Required = true,
                    })
                )
            );

        }
    }
}
