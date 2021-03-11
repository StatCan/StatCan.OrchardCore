using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.SurveyJS
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
            CreateSurveyJS();
            return 1;
        }

        private void CreateSurveyJS() {
            _contentDefinitionManager.AlterTypeDefinition("SurveyJS", type => type
                .DisplayedAs("SurveyJS")
                .Stereotype("Widget")
                .WithPart("SurveyJS", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("SurveyJS", part => part
                .WithField("Data", field => field
                    .OfType("TextField")
                    .WithDisplayName("Data")
                    .WithEditor("CodeMirror")
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
