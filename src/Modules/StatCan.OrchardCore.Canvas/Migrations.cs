using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;


namespace StatCan.OrchardCore.Canvas
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
            CreateCanvas();
            return 1;
        }

        private void CreateCanvas() {
            _contentDefinitionManager.AlterTypeDefinition("Canvas", type => type
            .DisplayedAs("Canvas")
            .Stereotype("Widget")
            .WithPart("Canvas", part => part
                .WithPosition("0")
            )
        );

        _contentDefinitionManager.AlterPartDefinition("Canvas", part => part
            .WithField("CanvasType", field => field
                .OfType("TextField")
                .WithDisplayName("CanvasType")
                .WithEditor("PredefinedList")
                .WithPosition("0")
                .WithSettings(new TextFieldSettings
                {
                    Hint = "Choose from the following options",
                    Required = true,
                })
                .WithSettings(new TextFieldPredefinedListEditorSettings
                {
                    Options = new ListValueOption[] {
                                    new ListValueOption() {
                                        Name="Business",
                                        Value="business"
                                    },
                                    new ListValueOption() {
                                        Name="Client",
                                        Value="client"
                                    },
                                     new ListValueOption() {
                                        Name="Community",
                                        Value="community"
                                    }
                    },
                })
            )
            .WithField("Data", field => field
                .OfType("TextField")
                .WithDisplayName("Data")
                .WithEditor("CodeMirror")
                .WithPosition("2")
                .WithSettings(new TextFieldSettings
                {
                    Hint = "Enter the json for the data.",
                    Required = true,
                })
            )
            .WithField("Colour", field => field
                .OfType("TextField")
                .WithDisplayName("Colour")
                .WithEditor("Color")
                .WithPosition("1")
                .WithSettings(new TextFieldSettings
                {
                    Hint = "Select a colour to match the website's theme",
                })
            )
        );
    }
}
}