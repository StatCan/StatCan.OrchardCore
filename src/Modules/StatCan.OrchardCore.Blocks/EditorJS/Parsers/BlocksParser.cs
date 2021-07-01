using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks;
using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.Fields;
using StatCan.OrchardCore.Blocks.Models;
using StatCan.OrchardCore.Blocks.Parsers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrchardCore.DisplayManagement;
using OrchardCore.Liquid;
using OrchardCore.Media;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers
{
    public class BlocksParser : IBlocksParser
    {
        #region Dependencies

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly ILogger<BlocksParser> _logger;
        private readonly IMediaFileStore _mediaFileStore;
        private readonly IShapeFactory _shapeFactory;

        private readonly IEnumerable<IBlockParser> _parsers;

        #endregion

        #region Constructor

        public BlocksParser(IHttpContextAccessor httpContextAccessor,
            ILiquidTemplateManager liquidTemplateManager,
            ILogger<BlocksParser> logger,
            IMediaFileStore mediaFileStore,
            IShapeFactory shapeFactory,
            IEnumerable<IBlockParser> parsers)
        {
            _httpContextAccessor = httpContextAccessor;
            _liquidTemplateManager = liquidTemplateManager;
            _logger = logger;
            _mediaFileStore = mediaFileStore;
            _shapeFactory = shapeFactory;
            _parsers = parsers;
        }

        #endregion

        #region Implementation

        public async Task<IList<dynamic>> RenderAsync(BlockField field)
        {
            return await RenderAsync(new BlockParserContext
            {
                ContentItem = field.ContentItem,
                HttpContext = _httpContextAccessor.HttpContext,
                LiquidTemplateManager = _liquidTemplateManager,
                MediaFileStore = _mediaFileStore,
                ShapeFactory = _shapeFactory
            }, field.Data);
        }

        public async Task<IList<dynamic>> RenderAsync(BlockBodyPart part)
        {
            return await RenderAsync(new BlockParserContext
            {
                ContentItem = part.ContentItem,
                HttpContext = _httpContextAccessor.HttpContext,
                LiquidTemplateManager = _liquidTemplateManager,
                MediaFileStore = _mediaFileStore,
                ShapeFactory = _shapeFactory
            }, part.Data);
        }

        #endregion

        #region Private Methods

        public async Task<IList<dynamic>> RenderAsync(BlockParserContext context, string data)
        {
            var shapes = new List<dynamic>();

            foreach (var block in JsonConvert.DeserializeObject<EditorBlocks>(data).Blocks)
            {
                var parser = _parsers.FirstOrDefault(p => p.Name == block.Type);
                if (parser == null)
                {
                    continue;
                }

                try
                {
                    shapes.Add(await parser.RenderAsync(context, block));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to render {block.Type} block.");
                }
            }

            return shapes;
        }

        #endregion
    }
}
