using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ContentOwnershipFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContentOwnershipFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var item = input.ToObjectValue() as ContentItem;

            if (item == null)
            {
                throw new ArgumentException("ContentItem missing while calling 'is_owner' filter");
            }

            var user = _httpContextAccessor.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return new ValueTask<FluidValue>(BooleanValue.False);
            }

            if (user.IsInRole("Administrator"))
            {
                return new ValueTask<FluidValue>(BooleanValue.True);
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if(userId == item.ContentItemId)
            {
                return new ValueTask<FluidValue>(BooleanValue.True);
            }

            return new ValueTask<FluidValue>(BooleanValue.False);
        }
    }
}
