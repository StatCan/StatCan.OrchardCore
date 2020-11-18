using Etch.OrchardCore.ContentPermissions.Models;
using Etch.OrchardCore.ContentPermissions.ViewModels;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Settings
{
    public class ContentPermissionsPartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, IUpdateModel updater)
        {
            if (!string.Equals(nameof(ContentPermissionsPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<ContentPermissionsPartSettingsViewModel>("ContentPermissionsPartSettings_Edit", model =>
            {
                var settings = contentTypePartDefinition.GetSettings<ContentPermissionsPartSettings>();

                model.RedirectUrl = settings.RedirectUrl;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!string.Equals(nameof(ContentPermissionsPart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new ContentPermissionsPartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.RedirectUrl);

            context.Builder.WithSettings(new ContentPermissionsPartSettings { RedirectUrl = model.RedirectUrl });

            return Edit(contentTypePartDefinition, context.Updater);
        }
    }
}
