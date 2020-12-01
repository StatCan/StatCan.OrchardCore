using Etch.OrchardCore.Blocks.Fields;
using Etch.OrchardCore.Blocks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.Parsers
{
    public interface IBlocksParser
    {
        Task<IList<dynamic>> RenderAsync(BlockField field);
        Task<IList<dynamic>> RenderAsync(BlockBodyPart part);
    }
}
