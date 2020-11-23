using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.ViewModels;

namespace StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings
{
    public class MultiValueTextFieldEditorSettingsDriver : ContentPartFieldDefinitionDisplayDriver<Fields.MultiValueTextField>
    {
        private readonly IStringLocalizer S;

        public MultiValueTextFieldEditorSettingsDriver(IStringLocalizer<MultiValueTextFieldEditorSettingsDriver> localizer)
        {
            S = localizer;
        }

        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<MultiValueTextFieldSettingsViewModel>("MultiValueTextFieldEditorSettings_Edit", model =>
            {
                var settings = partFieldDefinition.GetSettings<MultiValueTextFieldEditorSettings>();

                model.DefaultValue = settings.DefaultValue;
                model.Editor = settings.Editor;
                model.Options = JsonConvert.SerializeObject(settings.Options ?? new ListValueOption[0], Formatting.Indented);
            })
            .Location("Editor");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            if (partFieldDefinition.Editor() == "PredefinedList")
            {
                var model = new MultiValueTextFieldSettingsViewModel();
                var settings = new MultiValueTextFieldEditorSettings();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                try
                {
                    settings.DefaultValue = model.DefaultValue;
                    settings.Editor = model.Editor;
                    settings.Options = string.IsNullOrWhiteSpace(model.Options)
                        ? new ListValueOption[0]
                        : JsonConvert.DeserializeObject<ListValueOption[]>(model.Options);
                }
                catch
                {
                    context.Updater.ModelState.AddModelError(Prefix, S["The options are written in an incorrect format."]);
                    return Edit(partFieldDefinition);
                }

                context.Builder.WithSettings(settings);
            }

            return Edit(partFieldDefinition);
        }
    }
}
