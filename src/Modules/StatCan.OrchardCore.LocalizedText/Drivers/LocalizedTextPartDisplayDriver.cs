using StatCan.OrchardCore.LocalizedText.Fields;
using StatCan.OrchardCore.LocalizedText.Models;
using StatCan.OrchardCore.LocalizedText.ViewModels;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore.Localization;

namespace StatCan.OrchardCore.LocalizedText.Drivers
{
    public class LocalizedTextPartDisplayDriver : ContentPartDisplayDriver<LocalizedTextPart>
    {
        public IStringLocalizer T { get; set; }
        private readonly ILocalizationService _localizationService;

        public LocalizedTextPartDisplayDriver(IStringLocalizer<LocalizedTextPartDisplayDriver> localizer, ILocalizationService localizationService )
        {
            T = localizer;
            _localizationService = localizationService;
        }

        public async override Task<IDisplayResult> EditAsync(LocalizedTextPart part, BuildPartEditorContext context)
        {
            var cultures = (await _localizationService.GetSupportedCulturesAsync());

            return Initialize<EditLocalizedTextFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Part = part;
                model.SupportedCultures = JsonConvert.SerializeObject(cultures);
                model.Data = JsonConvert.SerializeObject(part.Data == null ? new List<LocalizedTextEntry>(): part.Data);

            });
        }

        public override async Task<IDisplayResult> UpdateAsync(LocalizedTextPart part, IUpdateModel updater, UpdatePartEditorContext context)
        {
            var model = new EditLocalizedTextFieldViewModel();

            await updater.TryUpdateModelAsync(model, Prefix, m => m.Data);

            part.Data = JsonConvert.DeserializeObject<List<LocalizedTextEntry>>(model.Data);

            return Edit(part, context);
        }

    }
}
