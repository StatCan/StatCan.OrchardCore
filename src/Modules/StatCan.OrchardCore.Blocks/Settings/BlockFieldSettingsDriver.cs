using StatCan.OrchardCore.Blocks.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.Settings
{
    public class BlockFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<BlockField>
    {
        public override IDisplayResult Edit(ContentPartFieldDefinition model)
        {
            return Initialize<BlockFieldSettings>("BlockFieldSettings_Edit", settings => model.PopulateSettings(settings))
                .Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition model, UpdatePartFieldEditorContext context)
        {
            var settings = new BlockFieldSettings();

            if (await context.Updater.TryUpdateModelAsync(settings, Prefix))
            {
                context.Builder.WithSettings(settings);
            }

            return Edit(model);
        }
    }
}
