using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class IsCurrentUrlFilter : ILiquidFilter
    {
        private readonly HttpContext _httpContext;

        public IsCurrentUrlFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, LiquidTemplateContext context)
        {
            var path = (_httpContext.Request.PathBase + _httpContext.Request.Path).ToString().TrimEnd('/');
            var toCompare = input.ToStringValue().TrimEnd('/');

            if(path.Equals(toCompare))
            {
                return new ValueTask<FluidValue>(BooleanValue.True);
            }
            return new ValueTask<FluidValue>(BooleanValue.False);
        }
    }
}
