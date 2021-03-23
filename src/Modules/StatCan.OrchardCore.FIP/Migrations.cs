using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;


namespace StatCan.OrchardCore.FIP
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
            CreateFip();
            return 1;
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
                        Options = new MultiTextFieldValueOption[] { new MultiTextFieldValueOption() {
                            Name = "Dark",
                            Value = "dark"
                        }, new MultiTextFieldValueOption() {
                            Name = "Light",
                            Value = "light"
                        }, new MultiTextFieldValueOption() {
                            Name = "Rounded",
                            Value = "rounded"
                        }, new MultiTextFieldValueOption() {
                            Name = "Shaped",
                            Value = "shaped"
                        } },
                    })
                )
            );
        }
    }
}