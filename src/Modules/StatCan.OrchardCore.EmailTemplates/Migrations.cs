using OrchardCore;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.EmailTemplates.Models;
using StatCan.OrchardCore.EmailTemplates.Services;
using System.Linq;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.EmailTemplates
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly EmailTemplatesManager _templatesManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager, EmailTemplatesManager templatesManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _templatesManager = templatesManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("EmailTemplatePart", builder => builder
                .WithDisplayName("Email Template")
                .WithDescription("Assign an email template to the content type")
                .Attachable());

            _contentDefinitionManager.MigratePartSettings<EmailTemplatePart, EmailTemplatePartSettings>();

            return 1;
        }

        public async Task<int> UpdateFrom1Async()
        {
            var templateDocuments = _templatesManager.GetEmailTemplatesDocumentAsync().Result;

            // if any EmailTemplates has a value on the name property it was already updated
            if (templateDocuments.Templates.Any(x => x.Value.Name != null))
            {
                return 2;
            }

            foreach (var templateDocument in templateDocuments.Templates)
            {
                // Assign all value to a new template, generate an Id and delete the old one.
                var template = new EmailTemplate
                {
                    Name = templateDocument.Key,
                    Description = templateDocument.Value.Description,
                    AuthorExpression = templateDocument.Value.AuthorExpression,
                    SenderExpression = templateDocument.Value.SenderExpression,
                    ReplyToExpression = templateDocument.Value.ReplyToExpression,
                    RecipientsExpression = templateDocument.Value.RecipientsExpression,
                    SubjectExpression = templateDocument.Value.SubjectExpression,
                    Body = templateDocument.Value.Body,
                    IsBodyHtml = templateDocument.Value.IsBodyHtml,
                };

                await _templatesManager.UpdateTemplateAsync(IdGenerator.GenerateId(), template);
                await _templatesManager.RemoveTemplateAsync(templateDocument.Key);

            }

            return 2;
        }
    }
}
