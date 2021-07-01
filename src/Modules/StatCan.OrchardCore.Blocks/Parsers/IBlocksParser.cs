using StatCan.OrchardCore.Blocks.Fields;
using StatCan.OrchardCore.Blocks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.Parsers
{
    public interface IBlocksParser
    {
        Task<IList<dynamic>> RenderAsync(BlockField field);
        Task<IList<dynamic>> RenderAsync(BlockBodyPart part);
    }
}
