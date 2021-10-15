using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System.Collections.Generic;
using System.Globalization;
using OrchardCore.ContentLocalization.Models;
using System.Collections;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ContentItemsFilter: ILiquidFilter
    {
        private readonly IContentManager _contentManager;

        public ContentItemsFilter(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var ids = input.ToObjectValue() as IEnumerable;
            var contentItems = new List<ContentItem>();

            foreach(var id in ids)
            {
                var contentItemId = id as string;
                var contentItem = await _contentManager.GetAsync(contentItemId);

                if(contentItem != null)
                {
                    contentItems.Add(contentItem);
                }
            }

            return FluidValue.Create(contentItems, context.Options);
        }
    }
}
