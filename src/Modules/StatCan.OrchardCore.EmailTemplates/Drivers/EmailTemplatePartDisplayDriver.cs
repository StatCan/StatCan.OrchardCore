using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.ViewModels;

namespace StatCan.OrchardCore.EmailTemplates.Drivers
{
    public class EmailTemplatePartDisplayDriver : ContentPartDisplayDriver<EmailTemplatePart>
    {
        public override IDisplayResult Display(EmailTemplatePart emailTemplatePart, BuildPartDisplayContext context)
        {
            var settings = context.TypePartDefinition.GetSettings<EmailTemplatePartSettings>();

            if (settings.SelectedEmailTemplates != null)
            {
                // This injects a button on the SummaryAdmin view for any ContentItem with an Email Template
                return Initialize<EmailTemplatePartViewModel>("EmailTemplatePart", m =>
                {
                    m.SelectedEmailTemplates = settings.SelectedEmailTemplates;
                    m.ContentItemId = emailTemplatePart.ContentItem.ContentItemId;
                })
                .Location("SummaryAdmin", "Actions:9");
            }
            return null;
        }
    }
}
