using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Fluid;
using Fluid.Values;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System.Collections;
using System.Collections.Generic;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ListOnlyFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var items = input.ToObjectValue() as IEnumerable;
            var contentItems = new List<ContentItem>();

            var limit = Decimal.ToInt32(arguments["limit"].ToNumberValue());
            var num = 0;

            foreach(var item in items)
            {
                var contentItem = item as ContentItem;

                if (contentItem == null)
                {
                    if (item is JObject jObject)
                    {
                        contentItem = jObject.ToObject<ContentItem>();
                    }
                }

                contentItems.Add(contentItem);
                num++;

                if(num == limit)
                {
                    break;
                }
            }

            return FluidValue.Create(contentItems, context.Options);
        }
    }
}
