using StatCan.OrchardCore.Blocks.EditorJS.Parsers;
using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks;
using StatCan.OrchardCore.Blocks.Parsers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Blocks.EditorJS
{
    [Feature("StatCan.OrchardCore.Blocks.EditorJS")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IBlocksParser, BlocksParser>();
            services.AddScoped<IBlockParser, DelimiterBlockParser>();
            services.AddScoped<IBlockParser, EmbedBlockParser>();
            services.AddScoped<IBlockParser, HeadingBlockParser>();
            services.AddScoped<IBlockParser, ImageParser>();
            services.AddScoped<IBlockParser, ParagraphBlockParser>();
            services.AddScoped<IBlockParser, QuoteBlockParser>();
            services.AddScoped<IBlockParser, RawBlockParser>();
        }
    }
}
