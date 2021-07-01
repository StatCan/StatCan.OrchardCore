using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public interface IBlockParser
    {
        string Name { get; }
        Task<dynamic> RenderAsync(BlockParserContext context, Block block);
    }
}
