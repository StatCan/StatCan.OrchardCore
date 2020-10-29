using System;
using System.Threading.Tasks;
using StatCan.OrchardCore.ContentFields.MultiSelect.Fields;
using StatCan.OrchardCore.ContentFields.MultiSelect.ViewModels;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;

namespace StatCan.OrchardCore.ContentFields.MultiSelect.Settings
{
    public class MultiSelectFieldSettingsDriver : ContentPartFieldDefinitionDisplayDriver<MultiSelectField>
    {
        #region Driver Methods

        #region Edit

        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<EditMultiSelectFieldSettingsViewModel>("MultiSelectFieldSettings_Edit", model =>
            {
                var settings = partFieldDefinition.GetSettings<MultiSelectFieldSettings>();

                model.Hint = settings.Hint;
                model.Options = settings.Options;
                model.OptionsJson = JsonConvert.SerializeObject(settings.Options ?? Array.Empty<string>());
            })
            .Location("Content");

        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            var model = new EditMultiSelectFieldSettingsViewModel();

            if (await context.Updater.TryUpdateModelAsync(model, Prefix))
            {
                context.Builder.WithSettings(new MultiSelectFieldSettings
                {
                    Hint = model.Hint,
                    Options = string.IsNullOrWhiteSpace(model.OptionsJson) ? Array.Empty<string>() : JsonConvert.DeserializeObject<string[]>(model.OptionsJson)
                });
            }

            return Edit(partFieldDefinition);
        }

        #endregion

        #endregion
    }
}

