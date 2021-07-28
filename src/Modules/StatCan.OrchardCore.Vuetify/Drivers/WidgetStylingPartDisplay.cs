using StatCan.OrchardCore.Vuetify.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Vuetify.Drivers
{
    public class WidgetStylingPartDisplay : ContentDisplayDriver
    {
        public override IDisplayResult Edit(ContentItem model, IUpdateModel updater)
        {
            var additionalStylingPart = model.As<WidgetStylingPart>();

            return additionalStylingPart == null
                ? null
                : Initialize<WidgetStylingPart>(
                    $"{nameof(WidgetStylingPart)}_Edit",
                    m =>
                    {
                        m.CustomClasses = additionalStylingPart.CustomClasses;
                    }).Location("Settings:3");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentItem model, IUpdateModel updater)
        {
            var additionalStylingPart = model.As<WidgetStylingPart>();

            if (additionalStylingPart == null)
            {
                return null;
            }

            await model.AlterAsync<WidgetStylingPart>(model => updater.TryUpdateModelAsync(model, Prefix));

            return await EditAsync(model, updater);
        }
    }
}
