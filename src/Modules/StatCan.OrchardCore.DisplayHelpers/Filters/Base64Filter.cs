using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class Base64Filter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, TemplateContext context)
        {
            var valueBytes = Encoding.UTF8.GetBytes(input.ToStringValue());
            return new ValueTask<FluidValue>(new StringValue(Convert.ToBase64String(valueBytes)));
        }
    }
}