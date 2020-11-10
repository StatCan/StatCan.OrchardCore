using System.Threading.Tasks;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;
using StatCan.OrchardCore.ContentFields.Multivalue.Settings;

namespace StatCan.OrchardCore.ContentFields.Multivalue.Settings
{
    public class MultivalueFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<MultivalueField>
    {
        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<MultivalueFieldSettings>("MultivalueFieldSettings_Edit", model => partFieldDefinition.PopulateSettings(model))
                .Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            var model = new MultivalueFieldSettings();

            await context.Updater.TryUpdateModelAsync(model, Prefix);

            context.Builder.WithSettings(model);

            return Edit(partFieldDefinition);
        }
    }
}
