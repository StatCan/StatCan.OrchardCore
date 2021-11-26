using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ParentContentItemIdFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ParentContentItemIdFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var path = input.ToObjectValue() as string;

            if (path == null)
            {
                throw new ArgumentException("Path missing while calling 'parent_contentitem_id' filter");
            }

            string[] pathValues = path.Substring(1).Split("/");

            if (pathValues.Length == 2)
            {
                return new ValueTask<FluidValue>(StringValue.Create(pathValues[pathValues.Length - 1]));
            }
            else if (pathValues.Length == 4)
            {
                return new ValueTask<FluidValue>(StringValue.Create(pathValues[1]));
            }

            return new ValueTask<FluidValue>(StringValue.Empty);
        }
    }
}
