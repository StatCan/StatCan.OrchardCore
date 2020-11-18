using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

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
                .WithFlow("1")
            );

            _contentDefinitionManager.AlterTypeDefinition("HtmlWidget", builder => builder
                .Stereotype("Widget")
                .WithHtmlBody("1")
            );

            _contentDefinitionManager.AlterTypeDefinition("LiquidWidget", builder => builder
                .Stereotype("Widget")
                .WithPart("LiquidPart", part => part
                    .WithDisplayName("Liquid Part")
                )
            );
             _contentDefinitionManager.AlterTypeDefinition("MarkdownWidget", builder => builder
                .Stereotype("Widget")
                .WithMarkdownBody("1")
            );
            return 1;

        }
    }
}