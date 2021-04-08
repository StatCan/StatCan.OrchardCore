using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class BoolFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var content = input.ToBooleanValue();
            return new ValueTask<FluidValue>(BooleanValue.Create(content));
        }
    }
}
