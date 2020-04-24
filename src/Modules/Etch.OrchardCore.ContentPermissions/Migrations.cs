using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Etch.OrchardCore.ContentPermissions
{
    public class Migrations : DataMigration
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;

        #endregion

        #region Constructor

        public Migrations(IContentDefinitionManager contentDefinitionManager)
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
