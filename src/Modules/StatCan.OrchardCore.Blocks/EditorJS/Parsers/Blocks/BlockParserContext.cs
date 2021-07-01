using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement;
using OrchardCore.Liquid;
using OrchardCore.Media;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class BlockParserContext
    {
        public ContentItem ContentItem { get; set; }
        public HttpContext HttpContext { get; set; }
        public ILiquidTemplateManager LiquidTemplateManager { get; set; }
        public IMediaFileStore MediaFileStore { get; set; }
        public IShapeFactory ShapeFactory { get; set; }
    }
}
