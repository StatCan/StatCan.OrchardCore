using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class JavascriptEncodeFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var rawValue = input.ToStringValue();
            if (!string.IsNullOrEmpty(rawValue))
            {
                return new StringValue(JavaScriptEncoder.Default.Encode(input.ToStringValue()), false);
            }
            return StringValue.Empty;
        }
    }
}
