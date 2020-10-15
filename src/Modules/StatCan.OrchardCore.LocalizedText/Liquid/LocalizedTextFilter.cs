using Fluid;
using Fluid.Values;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using StatCan.OrchardCore.LocalizedText.Fields;
using System.Linq;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.LocalizedText.Liquid
{
    public class LocalizedTextFilter : ILiquidFilter
    {
        private readonly ILocalizedTextAccessor _accessor;

        public LocalizedTextFilter(ILocalizedTextAccessor accessor)
        {
            _accessor = accessor;
        }
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var contentItem = input.ToObjectValue() as ContentItem;
            int paramFirstIndex = 0;
            var culture = context.CultureInfo.Name;
            var value = "";

            // if the ContentItem is passed to the filter, use the first argument as the name of the value to find
            if (contentItem != null)
            {
                var stringKey = arguments.At(0).ToStringValue();
                paramFirstIndex = 1;

                var part = contentItem.As<LocalizedTextPart>();
                if (part == null)
                {
                    return new ValueTask<FluidValue>(input);
                    //throw new ArgumentException(" The 'localize' filter requires the LocalizedTextPart");
                }

                value = part.Data.FirstOrDefault(lt => lt.Name == stringKey)?.LocalizedItems.FirstOrDefault(li => li.Culture == culture)?.Value;
            }
            else
            {
                // try to get the ContentItem from the accessor when the name of the value is passed as the input to the filter
                value = _accessor.GetTranslation(culture, input.ToStringValue());
            }

            if (!string.IsNullOrEmpty(value) && arguments.Count > 0)
            {
                var parameters = new object[arguments.Count];
                for (var i = paramFirstIndex; i < arguments.Count; i++)
                {
                    parameters[i] = arguments.At(i).ToStringValue();
                }
                value = string.Format(value, parameters);
            }

            return new ValueTask<FluidValue>(new StringValue(value));

        }
    }
}
