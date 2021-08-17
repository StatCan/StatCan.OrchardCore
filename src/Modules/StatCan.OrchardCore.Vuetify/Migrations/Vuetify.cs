using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    public class VuetifyMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public VuetifyMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            return 1;
        }
    }
}
