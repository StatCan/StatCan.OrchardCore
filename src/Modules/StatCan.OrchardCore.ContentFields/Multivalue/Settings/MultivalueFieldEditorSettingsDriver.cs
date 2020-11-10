using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;
using StatCan.OrchardCore.ContentFields.Multivalue.ViewModels;
using StatCan.OrchardCore.ContentFields.Multivalue.Settings;

namespace StatCan.OrchardCore.ContentFields.Multivalue.Settings
{
    public class MultivalueFieldEditorSettingsDriver : ContentPartFieldDefinitionDisplayDriver<MultivalueField>
    {
        private readonly IStringLocalizer S;

        public MultivalueFieldEditorSettingsDriver(IStringLocalizer<MultivalueFieldEditorSettingsDriver> localizer)
        {
            S = localizer;
        }

        public override IDisplayResult Edit(ContentPartFieldDefinition partFieldDefinition)
        {
            return Initialize<MultivalueFieldSettingsViewModel>("MultivalueFieldEditorSettings_Edit", model =>
            {
                var settings = partFieldDefinition.GetSettings<MultivalueFieldEditorSettings>();

                model.DefaultValue = settings.DefaultValue;
                model.Options = JsonConvert.SerializeObject(settings.Options ?? new ListValueOption[0], Formatting.Indented);
            })
            .Location("Editor");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartFieldDefinition partFieldDefinition, UpdatePartFieldEditorContext context)
        {
            if (partFieldDefinition.Editor() == "Multivalue")
            {
                var model = new MultivalueFieldSettingsViewModel();
                var settings = new MultivalueFieldEditorSettings();

                await context.Updater.TryUpdateModelAsync(model, Prefix);

                try
                {
                    settings.DefaultValue = model.DefaultValue;
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
