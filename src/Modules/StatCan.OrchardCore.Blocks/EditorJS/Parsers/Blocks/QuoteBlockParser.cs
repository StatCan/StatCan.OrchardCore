using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class QuoteBlockParser : IBlockParser
    {
        public string Name { get{ return "quote"; } }
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__Quote(
                new QuoteBlockViewModel
                {
                    Alignment = block.Get("alignment"),
                    Caption = block.Get("caption"),
                    Text = block.Get("text")
                }
            );
        }
    }
}
