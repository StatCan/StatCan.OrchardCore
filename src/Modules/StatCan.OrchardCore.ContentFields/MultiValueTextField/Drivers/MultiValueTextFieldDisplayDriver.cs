using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.ContentFields.ViewModels;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Mvc.ModelBinding;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Fields;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.ViewModels;

namespace StatCan.OrchardCore.ContentFields.MultiValueTextField.Drivers
{
    public class MultiValueTextFieldDisplayDriver : ContentFieldDisplayDriver<Fields.MultiValueTextField>
    {
        private readonly IContentManager _contentManager;
        private readonly IStringLocalizer S;

        public MultiValueTextFieldDisplayDriver(
            IContentManager contentManager,
            IStringLocalizer<MultiValueTextFieldDisplayDriver> localizer)
        {
            _contentManager = contentManager;
            S = localizer;
        }

        public override IDisplayResult Display(Fields.MultiValueTextField field, BuildFieldDisplayContext context)
        {
            return Initialize<DisplayMultiValueTextFieldViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            })
            .Location("Detail", "Content")
            .Location("Summary", "Content");
        }

        public override IDisplayResult Edit(Fields.MultiValueTextField field, BuildFieldEditorContext context)
        {
            return Initialize<EditMultiValueTextFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Values = field.Values;

                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;

                model.SelectedItems = new List<VueMultiselectItemViewModel>();

                foreach (var value in field.Values)
                {
                    model.SelectedItems.Add(new VueMultiselectItemViewModel
                    {
                        DisplayText = value.ToString(),
                    });
                }
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(Fields.MultiValueTextField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditMultiValueTextFieldViewModel();

            var modelUpdated = await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Values);

            if (!modelUpdated)
            {
                return Edit(field, context);
            }

            field.Values = viewModel.Values;

            var settings = context.PartFieldDefinition.GetSettings<MultiValueTextFieldSettings>();

            if (settings.Required && field.Values.Length == 0)
            {
                updater.ModelState.AddModelError(Prefix, nameof(field.Values), S["The {0} field is required.", context.PartFieldDefinition.DisplayName()]);
            }

            return Edit(field, context);
        }
    }
}
