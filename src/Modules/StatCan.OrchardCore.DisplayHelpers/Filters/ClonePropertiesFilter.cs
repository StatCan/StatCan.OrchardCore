using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.DisplayManagement;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class ClonePropertiesFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext ctx)
        {
          if (input.ToObjectValue() is IShape inputShape && arguments.At(0).ToObjectValue() is IShape toClone)
          {
            foreach (var prop in toClone.Properties)
            {
              if(!inputShape.Properties.ContainsKey(prop.Key))
              {
                inputShape.Properties.Add(prop.Key, prop.Value);
              }
            }
            return new ValueTask<FluidValue>(FluidValue.Create(inputShape, ctx.Options));
          }
          return new ValueTask<FluidValue>(input);
        }
    }
}
