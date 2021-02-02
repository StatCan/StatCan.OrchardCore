using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Assessment
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
            CreateAssessment();
            return 1;
        }

        private void CreateAssessment() {
            _contentDefinitionManager.AlterTypeDefinition("Assessment", type => type
                .DisplayedAs("Assessment")
                .Stereotype("Widget")
                .WithPart("Assessment", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Assessment", part => part
                .WithField("Data", field => field
                    .OfType("TextField")
                    .WithDisplayName("Data")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Insert JSON here",
                        Required = true,
                    })
                )
            );
        }
    }
}
