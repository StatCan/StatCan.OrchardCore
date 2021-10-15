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
    public class CurrentCultureFilter: ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var items = input.ToObjectValue() as IEnumerable;
            var contentItems = new List<ContentItem>();

            foreach (var item in items)
            {
                var contentItem = item as ContentItem;
                var part = contentItem.As<LocalizationPart>();

                if (part != null && part.Culture == CultureInfo.CurrentCulture.Name)
                {
                    contentItems.Add(contentItem);
                }
            }

            return FluidValue.Create(contentItems, context.Options);
        }
    }
}
