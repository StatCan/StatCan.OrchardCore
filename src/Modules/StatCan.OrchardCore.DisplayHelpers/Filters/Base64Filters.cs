using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class B64EncodeFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var toEncode = input.ToStringValue();
            return string.IsNullOrEmpty(toEncode) ? StringValue.Empty : new StringValue(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(toEncode)));
        }
    }
    public class B64DecodeFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var toDecode = input.ToStringValue();
            return string.IsNullOrEmpty(toDecode) ? StringValue.Empty : new StringValue(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(toDecode)));
        }
    }
}
