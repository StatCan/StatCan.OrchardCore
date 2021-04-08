using System.Security.Claims;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class GetClaimLiquidFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var claimType = arguments["type"].Or(arguments.At(0)).ToStringValue();

            if (input.ToObjectValue() is ClaimsPrincipal principal && principal.FindFirst(claimType) != null)
            {
                return new ValueTask<FluidValue>(new StringValue(principal.FindFirst(claimType).Value));
            }
            return new ValueTask<FluidValue>(new StringValue(""));
        }
    }
}
