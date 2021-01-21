using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Services;
using StatCan.OrchardCore.EmailTemplates.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.EmailTemplates.Drivers
{
    public class EmailTemplatePartSettingsDisplayDriver : ContentTypePartDefinitionDisplayDriver
    {
        private readonly EmailTemplatesManager _templatesManager;
        public EmailTemplatePartSettingsDisplayDriver(EmailTemplatesManager templatesManager)
        {
            _templatesManager = templatesManager;
        }

        public override IDisplayResult Edit(ContentTypePartDefinition contentTypePartDefinition, IUpdateModel updater)
        {
            if (!String.Equals(nameof(EmailTemplatePart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            return Initialize<EmailTemplatePartSettingsViewModel>("EmailTemplatePartSettings_Edit", async model =>
            {
                var settings = contentTypePartDefinition.GetSettings<EmailTemplatePartSettings>();
                var templateDocuments = await _templatesManager.GetEmailTemplatesDocumentAsync();
                

                if (settings.EmailTemplate != null)
                {
                    model.EmailTemplate = settings.EmailTemplate;
                }

                model.EmailTemplates = new List<SelectListItem>();

                foreach (var templateDocument in templateDocuments.Templates)
                {
                    model.EmailTemplates.Add(new SelectListItem
                    {
                        Value = templateDocument.Key,
                        Text = templateDocument.Key
                    });
                }

                model.EmailTemplatePartSettings = settings;
            }).Location("Content"); ;
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentTypePartDefinition contentTypePartDefinition, UpdateTypePartEditorContext context)
        {
            if (!String.Equals(nameof(EmailTemplatePart), contentTypePartDefinition.PartDefinition.Name))
            {
                return null;
            }

            var model = new EmailTemplatePartSettingsViewModel();

            await context.Updater.TryUpdateModelAsync(model, Prefix,
                m => m.EmailTemplate);

            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();
            context.Builder.WithSettings(new EmailTemplatePartSettings
            {
                EmailTemplate = model.EmailTemplate
            });

            return Edit(contentTypePartDefinition, context.Updater);
        }
    }
}
