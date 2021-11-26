using System.Security.Claims;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.Models;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class RemoveUnviewableContentFilter : ILiquidFilter
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContentManager _contentManager;

        public RemoveUnviewableContentFilter(IHttpContextAccessor httpContextAccessor, IContentManager contentManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _contentManager = contentManager;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            var items = input.ToObjectValue() as IEnumerable;
            var contentItems = new List<ContentItem>();

            foreach (var item in items)
            {
                var contentItem = item as ContentItem;

                if (contentItem == null)
                {
                    if (item is JObject jObject)
                    {
                        contentItem = jObject.ToObject<ContentItem>();
                    }
                }

                var permissionPart = contentItem.As<RadarPermissionPart>();

                if (!string.IsNullOrEmpty(permissionPart.ParentContentItemId))
                {
                    var parent = await _contentManager.GetAsync(permissionPart.ParentContentItemId);

                    var parentPermission = parent.As<RadarPermissionPart>();

                    if (!parentPermission.Published && !(userId == parentPermission.Owner) && !user.IsInRole("Administrator"))
                    {
                        continue;
                    }
                }

                if (!permissionPart.Published && !(userId == permissionPart.Owner) && !user.IsInRole("Administrator"))
                {
                    continue;
                }

                contentItems.Add(contentItem);
            }

            return FluidValue.Create(contentItems, context.Options);
        }
    }
}
