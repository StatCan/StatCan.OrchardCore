using System;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Newtonsoft.Json;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers
{
    public class JsonIndentedFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            switch (input.Type)
            {
                case FluidValues.Array:
                    return new ValueTask<FluidValue>(new StringValue(JsonConvert.SerializeObject(input.Enumerate().Select(o => o.ToObjectValue()),Formatting.Indented)));

                case FluidValues.Boolean:
                    return new ValueTask<FluidValue>(new StringValue(JsonConvert.SerializeObject(input.ToBooleanValue(), Formatting.Indented)));

                case FluidValues.Nil:
                    return new ValueTask<FluidValue>(StringValue.Create("null"));

                case FluidValues.Number:
                    return new ValueTask<FluidValue>(new StringValue(JsonConvert.SerializeObject(input.ToNumberValue(), Formatting.Indented)));

                case FluidValues.DateTime:
                case FluidValues.Dictionary:
                case FluidValues.Object:
                    return new ValueTask<FluidValue>(new StringValue(JsonConvert.SerializeObject(input.ToObjectValue(), Formatting.Indented)));

                case FluidValues.String:
                    var stringValue = input.ToStringValue();

                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return new ValueTask<FluidValue>(input);
                    }

                    return new ValueTask<FluidValue>(new StringValue(JsonConvert.SerializeObject(stringValue, Formatting.Indented)));
            }

            throw new NotSupportedException("Unrecognized FluidValue");
        }
    }
}
