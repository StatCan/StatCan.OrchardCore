using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Services;
using StatCan.OrchardCore.EmailTemplates.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace StatCan.OrchardCore.EmailTemplates.Drivers
{
    public class EmailTemplatePartDisplayDriver : ContentPartDisplayDriver<EmailTemplatePart>
    {
        private readonly EmailTemplatesManager _templatesManager;
        public EmailTemplatePartDisplayDriver(EmailTemplatesManager templatesManager)
        {
            _templatesManager = templatesManager;
        }

        public override IDisplayResult Display(EmailTemplatePart emailTemplatePart, BuildPartDisplayContext context)
        {
            var settings = context.TypePartDefinition.GetSettings<EmailTemplatePartSettings>();

            if (settings.SelectedEmailTemplates != null)
            {
                var templateDocuments = _templatesManager.GetEmailTemplatesDocumentAsync().Result;
                var emailTemplates = new List<SelectListItem>();

                foreach (var templateDocument in templateDocuments.Templates)
                {
                    emailTemplates.Add(new SelectListItem
                    {
                        Value = templateDocument.Key,
                        Text = templateDocument.Value.Name
                    });
                }

                // This injects a button on the SummaryAdmin view for any ContentItem with an Email Template
                return Initialize<EmailTemplatePartViewModel>("EmailTemplatePart", m =>
                {
                    m.SelectedEmailTemplates = emailTemplates.Where(x => settings.SelectedEmailTemplates.Contains(x.Value)).ToList();
                    m.ContentItemId = emailTemplatePart.ContentItem.ContentItemId;
                })
                .Location("SummaryAdmin", "Actions:9");
            }
            return null;
        }
    }
}
