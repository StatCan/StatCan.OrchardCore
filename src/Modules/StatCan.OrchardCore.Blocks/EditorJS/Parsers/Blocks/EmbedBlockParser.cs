using System.Threading.Tasks;
using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class EmbedBlockParser : IBlockParser
    {
        public string Name { get{ return "embed"; } }
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
