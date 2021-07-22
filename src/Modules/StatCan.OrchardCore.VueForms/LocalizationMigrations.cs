using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.VueForms
{

    public class LocalizationMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public LocalizationMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            // Weld the LocalizedText part
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                 .WithPart("LocalizedTextPart", p => p.WithPosition("4"))
            );

            return 1;
        }
    }
}
