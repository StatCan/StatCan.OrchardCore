using Etch.OrchardCore.Blocks.EditorJS.Parsers.Models;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public interface IBlockParser
    {
        Task<dynamic> RenderAsync(BlockParserContext context, Block block);
    }
}
