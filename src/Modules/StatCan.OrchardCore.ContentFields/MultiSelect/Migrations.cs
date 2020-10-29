using StatCan.OrchardCore.ContentFields.MultiSelect.Fields;
using StatCan.OrchardCore.ContentFields.MultiSelect.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.ContentFields.MultiSelect
{
    public class Migrations : DataMigration
    {
        #region Dependencies

        private readonly IContentDefinitionManager _contentDefinitionManager;

        #endregion Dependencies

        #region Constructor

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        #endregion Constructor

        #region Migrations

        public int Create()
        {
            _contentDefinitionManager.MigrateFieldSettings<MultiSelectField, MultiSelectFieldSettings>();

            return 1;
        }

        #endregion Migrations
    }
}
