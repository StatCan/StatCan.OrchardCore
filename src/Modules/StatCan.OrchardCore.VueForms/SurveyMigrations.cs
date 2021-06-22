using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.VueForms
{

    public class SurveyMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public SurveyMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("VueFormSurvey", part => part
                .WithField("SurveyJson", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("SurveyJson")
                    .WithSettings(new TextFieldSettings() { Hint = "Json output of the SurveyJs Creator (https://surveyjs.io/create-survey). With liquid support." })
                    .WithPosition("4")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"json\"}"
                        })
                )
                .WithDescription("Adds SurveyJS fields to VueForm"));

            _contentDefinitionManager.AlterTypeDefinition("VueForm", type =>
                type.WithPart("VueFormSurvey", p => p.WithPosition("5"))
            );


            return 1;
        }
    }
}
