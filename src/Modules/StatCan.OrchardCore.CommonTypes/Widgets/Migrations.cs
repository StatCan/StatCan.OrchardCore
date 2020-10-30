using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;

namespace StatCan.OrchardCore.CommonTypes.Widgets
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
            _contentDefinitionManager.AlterTypeDefinition("Container", builder => builder
                .Stereotype("Widget")
                .WithPart("FlowPart", part => part.WithPosition("1"))
            );

            _contentDefinitionManager.AlterTypeDefinition("HtmlWidget", builder => builder
                .Stereotype("Widget")
                .WithPart("HtmlBodyPart", part => part
                    .WithDisplayName("HTML Body")
                    .WithEditor("Wysiwyg")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("LiquidWidget", builder => builder
                .Stereotype("Widget")
                .WithPart("LiquidPart", part => part
                    .WithDisplayName("Liquid Part")
                )
            );
             _contentDefinitionManager.AlterTypeDefinition("MarkdownWidget", builder => builder
                .Stereotype("Widget")
                .WithPart("MarkdownBodyPart", part => part
                    .WithDisplayName("Markdown body")
                    .WithEditor("Wysiwyg")
                )
            );
            return 1;

        }
    }
}