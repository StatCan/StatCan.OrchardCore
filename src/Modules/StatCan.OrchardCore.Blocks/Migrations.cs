using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;

namespace Etch.OrchardCore.Blocks
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
            _contentDefinitionManager.AlterPartDefinition("BlockBodyPart", builder => builder
                .Attachable()
                .Reusable()
                .WithDescription("Provides rich text editor for curating content for body of content item.")
                .WithDefaultPosition("5")
            );

            return 1;
        }

        #endregion
    }
}
