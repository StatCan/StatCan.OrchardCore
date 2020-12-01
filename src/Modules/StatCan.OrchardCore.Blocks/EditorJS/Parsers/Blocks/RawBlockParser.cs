using Etch.OrchardCore.Blocks.EditorJS.Parsers.Models;
using Etch.OrchardCore.Blocks.ViewModels.Blocks;
using Fluid;
using OrchardCore.Liquid;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class RawBlockParser : IBlockParser
    {
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            var templateContext = new TemplateContext();
            templateContext.SetValue("ContentItem", context.ContentItem);

            return await context.ShapeFactory.New.Block__Raw(
                new RawBlockViewModel
                {
                    Html = await context.LiquidTemplateManager.RenderAsync(block.Get("html"), HtmlEncoder.Default, templateContext)
                }
            ); ;
        }
    }
}
