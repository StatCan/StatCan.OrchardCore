using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.LocalizedText
{
    public class Migrations : DataMigration
    {
        private IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("LocalizedTextPart", builder => builder
                .Attachable()
                .WithDescription("Provides inline localization text"));

            return 1;
        }
    }
}
