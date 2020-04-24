using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions
{
    public class CanViewLiquidFilter : ILiquidFilter
    {

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var ret = false;
            var roles = arguments.At(0).ToObjectValue() as string[];
            if (roles != null && input.ToObjectValue() is ClaimsPrincipal principal)
            {
                ret = principal.CanAccess(roles);
            }

            return new ValueTask<FluidValue>(ret ? BooleanValue.True : BooleanValue.False);
        }
    }
}
