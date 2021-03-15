using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.Workflows.Display;
using OrchardCore.Workflows.Models;
using StatCan.OrchardCore.EmailTemplates.Services;
using StatCan.OrchardCore.EmailTemplates.Workflows.Activities;
using StatCan.OrchardCore.EmailTemplates.Workflows.ViewModels;
using System.Collections.Generic;

namespace StatCan.OrchardCore.EmailTemplates.Workflows.Drivers
{
    class EmailTemplateTaskDisplayDriver : ActivityDisplayDriver<EmailTemplateTask, EmailTemplateTaskViewModel>
    {
        private readonly EmailTemplatesManager _templatesManager;

        public EmailTemplateTaskDisplayDriver(EmailTemplatesManager templatesManager)
        {
            _templatesManager = templatesManager;
        }

        protected override void EditActivity(EmailTemplateTask activity, EmailTemplateTaskViewModel model)
        {
            var templateDocuments = _templatesManager.GetEmailTemplatesDocumentAsync().Result;
            model.EmailTemplates = new List<SelectListItem>();

            foreach (var templateDocument in templateDocuments.Templates)
            {
                model.EmailTemplates.Add(new SelectListItem
                {
                    Value = templateDocument.Key,
                    Text = templateDocument.Value.Name
                });
            }

            model.SelectedEmailTemplateId = activity.SelectedEmailTemplateId.Expression;
            model.TemplateModelExpression = activity.TemplateModelExpression.Expression;
        }

        protected override void UpdateActivity(EmailTemplateTaskViewModel model, EmailTemplateTask activity)
        {
            activity.SelectedEmailTemplateId = new WorkflowExpression<string>(model.SelectedEmailTemplateId);
            activity.TemplateModelExpression = new WorkflowExpression<dynamic>(model.TemplateModelExpression);
        }
    }
}
