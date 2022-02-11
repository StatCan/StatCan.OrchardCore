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

        // General idea is that only owner can see own draft item and admin can see everything
        public override async Task<IDisplayResult> DisplayAsync(RadarPermissionPart part, BuildPartDisplayContext context)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            if (!string.IsNullOrEmpty(part.ParentContentItemId))
            {
                var parent = await _contentManager.GetAsync(part.ParentContentItemId);

                var parentPermission = parent.As<RadarPermissionPart>();

                if (!parentPermission.Published && !(userId == parentPermission.Owner) && !user.IsInRole("Administrator"))
                {
                    _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                    _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
                }
            }

            if (part.Published)
            {
                return null;
            }

            // Assume admin owns everything
            if (user.IsInRole("Administrator"))
            {
                return null;
            }

            if (!(userId == part.Owner))
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = 404;
                _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found", false);
            }

            return null;
        }
    }
}
