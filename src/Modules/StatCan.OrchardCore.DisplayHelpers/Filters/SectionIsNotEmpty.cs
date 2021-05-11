using Fluid;
using Fluid.Values;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.DisplayManagement.Layout;
using OrchardCore.Liquid;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class SectionIsNotEmpty : ILiquidFilter
    {
        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var layoutAccessor = context.Services.GetRequiredService<ILayoutAccessor>();
            var layout = await layoutAccessor.GetLayoutAsync();

            return BooleanValue.Create(layout.Zones.IsNotEmpty(input.ToStringValue()));
        }
    }
}
