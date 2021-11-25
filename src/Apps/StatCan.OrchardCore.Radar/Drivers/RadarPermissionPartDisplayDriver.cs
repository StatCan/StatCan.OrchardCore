using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.ContentManagement;
using StatCan.OrchardCore.Radar.Models;
using StatCan.OrchardCore.Radar.Helpers;

namespace StatCan.OrchardCore.Radar.Drivers
{
    public class RadarPermissionPartDisplayDriver : ContentPartDisplayDriver<RadarPermissionPart>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContentManager _contentManager;

        public RadarPermissionPartDisplayDriver(IHttpContextAccessor httpContextAccessor, IContentManager contentManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _contentManager = contentManager;
        }

        public override async Task<IDisplayResult> DisplayAsync(RadarPermissionPart part, BuildPartDisplayContext context)
        {
            var contentItem = await _contentManager.GetAsync(part.ContentItemId);

            if (contentItem.Content.RadarEntityPart.Publish.Value.ToObject<bool>())
            {
               return null;
            }

            var user = _httpContextAccessor.HttpContext.User;

            if (!Ownership.IsOwner(contentItem, user))
            {
               _httpContextAccessor.HttpContext.Response.StatusCode = 404;
               _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
            }

            return null;
        }
    }
}
