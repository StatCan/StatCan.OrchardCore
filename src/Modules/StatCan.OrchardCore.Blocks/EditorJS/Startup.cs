using Etch.OrchardCore.Blocks.EditorJS.Parsers;
using Etch.OrchardCore.Blocks.Parsers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace Etch.OrchardCore.Blocks.EditorJS
{
    [Feature("StatCan.OrchardCore.Blocks.EditorJS")]
    public class Startup : StartupBase
    {
        static Startup()
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBlocksParser, BlocksParser>();
        }
    }
}
