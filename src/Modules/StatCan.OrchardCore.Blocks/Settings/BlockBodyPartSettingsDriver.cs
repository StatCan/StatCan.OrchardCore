using StatCan.OrchardCore.Blocks.Models;
using StatCan.OrchardCore.Blocks.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.Settings
{
    public class BlockBodyPartSettingsDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition model)
        {
            if (!string.Equals(nameof(BlockBodyPart), model.PartDefinition.Name, StringComparison.Ordinal))
            {
                return null;
            }

            return Initialize<BlockBodyPartSettingsViewModel>("BlockBodyPartSettings_Edit", settings =>
            {
                var blockBodyPartSettings = model.GetSettings<BlockBodyPartSettings>();

                settings.LinkableContentTypes = blockBodyPartSettings.LinkableContentTypes;
                settings.BlockBodyPartSettings = blockBodyPartSettings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition model, UpdateTypePartEditorContext context)
        {
            if (!string.Equals(nameof(BlockBodyPart), model.PartDefinition.Name, StringComparison.Ordinal))
            {
                return null;
            }

            var settings = new BlockBodyPartSettings();

            await context.Updater.TryUpdateModelAsync(settings, Prefix);

            context.Builder.WithSettings(settings);

            return Edit(model);
        }
    }
}
