using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.ViewModels;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace StatCan.OrchardCore.Candev
{
    public class CandevDriver : ContentDisplayDriver
    {
        public override IDisplayResult Display(ContentItem model, IUpdateModel updater)
        {
            if (model.ContentType == "Team")
            {
                // This injects a button on the SummaryAdmin view for the Team ContentType
                return Combine(
                  Shape("Content_SummaryAdmin__Team__Buttons", new ContentItemViewModel(model)).Location("SummaryAdmin", "Actions:9")
                );
            }
            return null;
        }
    }
}
