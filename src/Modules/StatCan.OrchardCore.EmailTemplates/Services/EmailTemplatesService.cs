using OrchardCore.ContentManagement;
using OrchardCore.Email;
using OrchardCore.Liquid;
using StatCan.OrchardCore.EmailTemplates.ViewModels;
using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.EmailTemplates.Services
{
    public class EmailTemplatesService : IEmailTemplatesService
    {
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly HtmlEncoder _htmlEncoder;

        public EmailTemplatesService(
            ILiquidTemplateManager liquidTemplateManager,
            HtmlEncoder htmlEncoder
        )
        {
            _liquidTemplateManager = liquidTemplateManager;
            _htmlEncoder = htmlEncoder;
        }

        public MailMessage CreateMessageFromViewModel(SendEmailTemplateViewModel sendEmail)
        {
            var message = new MailMessage
            {
                To = sendEmail.Recipients,
                Bcc = sendEmail.BCC,
                Cc = sendEmail.CC,
                ReplyTo = sendEmail.ReplyTo
            };

            var author = sendEmail.Author;
            var sender = sendEmail.Sender;

            if (!string.IsNullOrWhiteSpace(author))
            {
                message.From = author.Trim();
            }

            if (!String.IsNullOrWhiteSpace(sender))
            {
                message.Sender = sender.Trim();
            }

            if (!String.IsNullOrWhiteSpace(sendEmail.Subject))
            {
                message.Subject = sendEmail.Subject;
            }

            if (!String.IsNullOrWhiteSpace(sendEmail.Body))
            {
                message.Body = sendEmail.Body;
            }

            message.IsBodyHtml = sendEmail.IsBodyHtml;

            return message;
        }

        public Task<string> RenderLiquid(string liquid, object model)
        {
            if (!string.IsNullOrWhiteSpace(liquid))
            {
                return _liquidTemplateManager.RenderStringAsync(liquid, _htmlEncoder, model, null);
            }
            return Task.FromResult(liquid);
        }
    }
}
