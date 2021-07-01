using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class HeadingBlockParser : IBlockParser
    {
        public string Name { get{ return "header"; } }
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__Heading(
                new HeadingBlockViewModel
                {
                    Level = block.Has("level") ? int.Parse(block.Get("level")) : 1,
                    Text = block.Get("text")
                }
            );
        }
    }
}
