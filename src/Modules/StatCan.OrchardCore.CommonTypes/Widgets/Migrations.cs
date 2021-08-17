using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.CommonTypes.Widgets
{
    public class HtmlMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public HtmlMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("HtmlWidget", builder => builder
                .Stereotype("Widget")
                .WithHtmlBody("1")
            );
            return 1;
        }
    }

    public class LiquidMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public LiquidMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterTypeDefinition("LiquidWidget", builder => builder
                .Stereotype("Widget")
                .WithPart("LiquidPart", part => part
                    .WithDisplayName("Liquid Part")
                )
            );
            return 1;
        }
    }
    public class MarkdownMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public MarkdownMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
             _contentDefinitionManager.AlterTypeDefinition("MarkdownWidget", builder => builder
                .Stereotype("Widget")
                .WithMarkdownBody("1")
            );
            return 1;

        }
    }

}
