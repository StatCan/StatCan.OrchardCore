using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class ParagraphBlockParser : IBlockParser
    {
        public string Name { get{ return "paragraph"; } }
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            if (string.IsNullOrWhiteSpace(block.Get("text")))
            {
                return await context.ShapeFactory.New.Block__LineBreak();
            }

            return await context.ShapeFactory.New.Block__Paragraph(
                new ParagraphBlockViewModel
                {
                    Text = BlockParserHelper.AddPathBaseToRelativeLinks(context.HttpContext.Request.PathBase, block.Get("text"))
                }
            );
        }
    }
}
