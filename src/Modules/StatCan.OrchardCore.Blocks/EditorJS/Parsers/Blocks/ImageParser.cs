﻿using StatCan.OrchardCore.Blocks.EditorJS.Parsers.Models;
using StatCan.OrchardCore.Blocks.ViewModels.Blocks;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.EditorJS.Parsers.Blocks
{
    public class ImageParser : IBlockParser
    {
        public string Name { get{ return "image"; } }
        public async Task<dynamic> RenderAsync(BlockParserContext context, Block block)
        {
            return await context.ShapeFactory.New.Block__Image(
                new ImageBlockViewModel
                {
                    Caption = block.Get("caption"),
                    Stretched = block.Get("stretched", false),
                    Url = GetMediaUrl(context, block)
                }
            );
        }

        private string GetMediaUrl(BlockParserContext context, Block block)
        {
            var mediaPath = block.Get("mediaPath");

            if (string.IsNullOrEmpty(mediaPath))
            {
                return block.Get("url");
            }

            return context.MediaFileStore.MapPathToPublicUrl(mediaPath);
        }
    }
}
