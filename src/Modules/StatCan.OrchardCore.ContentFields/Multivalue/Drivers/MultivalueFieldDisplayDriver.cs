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
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;
using StatCan.OrchardCore.ContentFields.Multivalue.Settings;
using StatCan.OrchardCore.ContentFields.Multivalue.ViewModels;

namespace StatCan.OrchardCore.ContentFields.Multivalue.Drivers
{
    public class MultivalueFieldDisplayDriver : ContentFieldDisplayDriver<MultivalueField>
    {
        private readonly IContentManager _contentManager;
        private readonly IStringLocalizer S;

        public MultivalueFieldDisplayDriver(
            IContentManager contentManager,
            IStringLocalizer<MultivalueFieldDisplayDriver> localizer)
        {
            _contentManager = contentManager;
            S = localizer;
        }

        public override IDisplayResult Display(MultivalueField field, BuildFieldDisplayContext context)
        {
            return Initialize<DisplayMultivalueFieldViewModel>(GetDisplayShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
            })
            .Location("Detail", "Content")
            .Location("Summary", "Content");
        }

        public override IDisplayResult Edit(MultivalueField field, BuildFieldEditorContext context)
        {
            return Initialize<EditMultivalueFieldViewModel>(GetEditorShapeType(context), model =>
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

        public override async Task<IDisplayResult> UpdateAsync(MultivalueField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            var viewModel = new EditMultivalueFieldViewModel();

            var modelUpdated = await updater.TryUpdateModelAsync(viewModel, Prefix, f => f.Values);

            if (!modelUpdated)
            {
                return Edit(field, context);
            }

            field.Values = viewModel.Values;

            var settings = context.PartFieldDefinition.GetSettings<MultivalueFieldSettings>();

            if (settings.Required && field.Values.Length == 0)
            {
                updater.ModelState.AddModelError(Prefix, nameof(field.Values), S["The {0} field is required.", context.PartFieldDefinition.DisplayName()]);
            }

            return Edit(field, context);
        }
    }
}
