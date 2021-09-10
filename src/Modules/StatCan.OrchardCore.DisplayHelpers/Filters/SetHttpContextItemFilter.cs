using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class SetHttpContextItemFilter : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var httpContext = context.Services.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

            if (httpContext != null)
            {
                httpContext.Items[input.ToStringValue()] = args.At(0).ToObjectValue();
            }

            return StringValue.Empty;
        }
    }
}
