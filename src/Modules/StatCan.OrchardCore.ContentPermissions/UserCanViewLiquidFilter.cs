using Etch.OrchardCore.ContentPermissions.Models;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions
{
    public class UserCanViewLiquidFilter : ILiquidFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCanViewLiquidFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var item = input.ToObjectValue() as ContentItem;
            if (item == null)
            {
                throw new ArgumentException("ContentItem missing while calling 'user_can_view' filter");
            }

            var part = item.As<ContentPermissionsPart>();
            if (part == null || !part.Enabled || part.Roles == null || part.Roles.Length == 0)
            {
                // if the part does not exist, is not enabled or the roles are empty return true
                return new ValueTask<FluidValue>(BooleanValue.True);
            }

            var user = _httpContextAccessor.HttpContext?.User;
            var ret = user.CanAccess(part.Roles);

            return new ValueTask<FluidValue>(ret ? BooleanValue.True : BooleanValue.False);
        }
    }
}
