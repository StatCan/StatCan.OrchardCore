using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.ViewModels;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;

namespace StatCan.OrchardCore.Hackathon
{
    public class HackathonDriver : ContentDisplayDriver
    {
        public override IDisplayResult Display(ContentItem model, IUpdateModel updater)
        {
            if (model.ContentType == "Team")
            {
                // This injects a button on the SummaryAdmin view for the Hacker ContentType
                return Combine(
                  Shape("Contents_SummaryAdmin__Team__MailTo__Button", new ContentItemViewModel(model)).Location("SummaryAdmin", "Actions:9")
                );
            }
            return null;
        }
    }
}
