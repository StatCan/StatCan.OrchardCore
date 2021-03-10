using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Email;
using OrchardCore.Liquid;
using OrchardCore.Workflows.Abstractions.Models;
using OrchardCore.Workflows.Activities;
using OrchardCore.Workflows.Models;
using OrchardCore.Workflows.Services;
using StatCan.OrchardCore.EmailTemplates.Services;
using StatCan.OrchardCore.EmailTemplates.ViewModels;

namespace StatCan.OrchardCore.EmailTemplates.Workflows.Activities
{
    public class EmailTemplateTask : TaskActivity
    {
        private readonly IStringLocalizer S;
        private readonly EmailTemplatesManager _templatesManager;
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly ISmtpService _smtpService;
        private readonly IEmailTemplatesService _emailTemplatesService;
        private readonly IWorkflowScriptEvaluator _scriptEvaluator;

        public EmailTemplateTask(
            IStringLocalizer<EmailTemplateTask> localizer,
            EmailTemplatesManager templatesManager,
            HtmlEncoder htmlEncoder,
            ILiquidTemplateManager liquidTemplateManager,
            ISmtpService smtpService,
            IEmailTemplatesService emailTemplatesService,
            IWorkflowScriptEvaluator scriptEvaluator
        )
        {
            S = localizer;
            _templatesManager = templatesManager;
            _htmlEncoder = htmlEncoder;
            _liquidTemplateManager = liquidTemplateManager;
            _smtpService = smtpService;
            _emailTemplatesService = emailTemplatesService;
            _scriptEvaluator = scriptEvaluator;
        }

        public override string Name => nameof(EmailTemplateTask);

        public string SelectedEmailTemplateName => GetSelectedEmailTemplateName();

        public WorkflowExpression<string> SelectedEmailTemplateId
        {
            get => GetProperty(() => new WorkflowExpression<string>());
            set => SetProperty(value);
        }

        public WorkflowExpression<dynamic> TemplateModelExpression
        {
            get => GetProperty(() => new WorkflowExpression<dynamic>());
            set => SetProperty(value);
        }

        public override LocalizedString DisplayText => S["Email Template Task"];

        public override LocalizedString Category => S["Messaging"];

        public override IEnumerable<Outcome> GetPossibleOutcomes(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            return Outcomes(S["Done"], S["Failed"]);
        }

        public override async Task<ActivityExecutionResult> ExecuteAsync(WorkflowExecutionContext workflowContext, ActivityContext activityContext)
        {
            var templatesDocument = await _templatesManager.GetEmailTemplatesDocumentAsync();

            if (!templatesDocument.Templates.ContainsKey(SelectedEmailTemplateId.Expression))
            {
                return Outcomes("Failed");
            }

            var template = templatesDocument.Templates[SelectedEmailTemplateId.Expression];

            dynamic contentItem = (object) _scriptEvaluator.EvaluateAsync(TemplateModelExpression, workflowContext).Result;

            var model = new SendEmailTemplateViewModel
            {
                Name = template.Name,
                Author = await _emailTemplatesService.RenderLiquid(template.AuthorExpression, contentItem),
                Sender = await _emailTemplatesService.RenderLiquid(template.SenderExpression, contentItem),
                ReplyTo = await _emailTemplatesService.RenderLiquid(template.ReplyToExpression, contentItem),
                Recipients = await _emailTemplatesService.RenderLiquid(template.RecipientsExpression, contentItem),
                Subject = await _emailTemplatesService.RenderLiquid(template.SubjectExpression, contentItem),
                Body = await _emailTemplatesService.RenderLiquid(template.Body, contentItem),
                IsBodyHtml = template.IsBodyHtml,
            };

            var message = _emailTemplatesService.CreateMessageFromViewModel(model);

            var result = await _smtpService.SendAsync(message);

            if (!result.Succeeded)
            {
                return Outcomes("Failed");
            }

            return Outcomes("Done");
        }

        private string GetSelectedEmailTemplateName()
        {
            var templatesDocument = _templatesManager.GetEmailTemplatesDocumentAsync().Result;

            if (templatesDocument.Templates.ContainsKey(SelectedEmailTemplateId.Expression))
            {
                return templatesDocument.Templates.Where(x => x.Key == SelectedEmailTemplateId.Expression).Select(x => x.Value.Name).FirstOrDefault();
            }

            return null;
        }
    }
}
