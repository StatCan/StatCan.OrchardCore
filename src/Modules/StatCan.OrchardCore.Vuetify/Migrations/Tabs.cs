using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Tabs)]
    public class TabsMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public TabsMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            Tabs();
            return 1;
        }
        private void Tabs()
        {
            _contentDefinitionManager.CreateBasicWidget("Tabs");
            _contentDefinitionManager.AlterTypeDefinition("Tabs", t => t
                .WithTitlePart("0")
                .WithFlow("1", new string[] { "Tab" })
            );
            _contentDefinitionManager.CreateBasicWidget("Tab");
            _contentDefinitionManager.AlterTypeDefinition("Tab", t => t
                .WithTitlePart("0", TitlePartOptions.EditableRequired)
                .WithFlow("2")
            );
        }
    }
}
