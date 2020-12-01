using System.Threading.Tasks;
using Etch.OrchardCore.Blocks.EditorJS.Parsers.Models;
using Etch.OrchardCore.Blocks.ViewModels.Blocks;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class EmbedBlockParser : IBlockParser
    {
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__Embed(
                new EmbedBlockViewModel
                {
                    Caption = block.Get("caption"),
                    Service = block.Get("service"),
                    SourceUrl = block.Get("embed")
                }
            );
        }
    }
}
