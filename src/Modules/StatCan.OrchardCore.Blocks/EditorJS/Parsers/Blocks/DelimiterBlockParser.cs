using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class DelimiterBlockParser : IBlockParser
    {
        public string Name { get{ return "delimiter"; } }
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__Delimiter();
        }
    }
}
