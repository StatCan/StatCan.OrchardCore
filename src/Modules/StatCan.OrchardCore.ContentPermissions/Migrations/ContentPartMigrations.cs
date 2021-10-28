using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Etch.OrchardCore.ContentPermissions.Migrations
{
    public class ContentPartMigrations : DataMigration
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;

        #endregion

        #region Constructor

        public ContentPartMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        #endregion

        #region Migrations

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("ContentPermissionsPart", builder => builder
                .Attachable()
                .WithDescription("Provides ability to control which roles can view content item.")
                .WithDisplayName("Content Permissions")
                .WithDefaultPosition("10")
            );

            return 1;
        }

        #endregion
    }
}
