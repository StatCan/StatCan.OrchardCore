using Etch.OrchardCore.Blocks.EditorJS.Parsers.Models;
using Etch.OrchardCore.Blocks.ViewModels.Blocks;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class HeadingBlockParser : IBlockParser
    {
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
