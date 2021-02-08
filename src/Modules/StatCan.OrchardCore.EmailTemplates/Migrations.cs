using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.EmailTemplates.Models;

namespace StatCan.OrchardCore.EmailTemplates
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
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
    }
}
