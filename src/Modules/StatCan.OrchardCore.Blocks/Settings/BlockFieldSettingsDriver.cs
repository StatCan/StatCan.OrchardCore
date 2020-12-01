using Etch.OrchardCore.Blocks.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.Settings
{
    public class BlockFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<BlockField>
    {
        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<BlockFieldSettings>("BlockFieldSettings_Edit", model => partFieldDefinition.PopulateSettings(model))
                .Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            var settings = new BlockFieldSettings();

            if (await context.Updater.TryUpdateModelAsync(settings, Prefix))
            {
                context.Builder.WithSettings(settings);
            }

            return Edit(partFieldDefinition);
        }
    }
}
