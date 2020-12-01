using Etch.OrchardCore.Blocks.Models;
using Etch.OrchardCore.Blocks.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.Settings
{
    public class BlockBodyPartSettingsDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition)
        {
            if (!string.Equals(nameof(BlockBodyPart), contentTypePartDefinition.PartDefinition.Name, StringComparison.Ordinal))
            {
                return null;
            }

            return Initialize<BlockBodyPartSettingsViewModel>("BlockBodyPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<BlockBodyPartSettings>();

                model.LinkableContentTypes = settings.LinkableContentTypes;
                model.BlockBodyPartSettings = settings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            var model = new BlockBodyPartSettings();

            await context.Updater.TryUpdateModelAsync(model, Prefix);

            context.Builder.WithSettings(model);

            return Edit(contentTypePartDefinition);
        }
    }
}
