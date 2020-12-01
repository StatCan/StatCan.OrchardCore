using Etch.OrchardCore.Blocks.EditorJS.Parsers.Models;
using Etch.OrchardCore.Blocks.ViewModels.Blocks;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class ListBlockParser : IBlockParser
    {
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__List(
                new ListBlockViewModel
                {
                    ListItems = block.Has("items") ? (block.Data["items"] as JArray).ToObject<string[]>() : new string[0],
                    Style = block.Get("style")
                }
            );
        }
    }
}
