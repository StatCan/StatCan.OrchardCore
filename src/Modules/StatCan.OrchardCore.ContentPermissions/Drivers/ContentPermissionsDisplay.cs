using Etch.OrchardCore.ContentPermissions.Models;
using Etch.OrchardCore.ContentPermissions.Services;
using Etch.OrchardCore.ContentPermissions.ViewModels;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Security.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Drivers
{
    public class ContentPermissionsDisplay : ContentPartDisplayDriver<ContentPermissionsPart>
    {
        #region Dependencies

        private readonly IContentPermissionsService _contentPermissionsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleService _roleService;

        #endregion

        #region Constructor

        public ContentPermissionsDisplay(IContentPermissionsService contentPermissionsService, IHttpContextAccessor httpContextAccessor, IRoleService roleService)
        {
            _contentPermissionsService = contentPermissionsService;
            _httpContextAccessor = httpContextAccessor;
            _roleService = roleService;
        }

        #endregion

        #region Overrides

        public override IDisplayResult Display(ContentPermissionsPart part, BuildPartDisplayContext context)
        {
            var settings = _contentPermissionsService.GetSettings(part);

            if (context.DisplayType != "Detail" || !settings.HasRedirectUrl || _contentPermissionsService.CanAccess(part))
            {
                return null;
            }

            var redirectUrl = settings.RedirectUrl;

            if (!settings.RedirectUrl.StartsWith("/"))
            {
                redirectUrl = $"/{redirectUrl}";
            }

            _httpContextAccessor.HttpContext.Response.StatusCode = 403;
            _httpContextAccessor.HttpContext.Response.Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}{redirectUrl}", false);
            return null;
        }

        public override async Task<IDisplayResult> EditAsync(ContentPermissionsPart part, BuildPartEditorContext context)
        {
            var roles = await _roleService.GetRoleNamesAsync();

            return Initialize<ContentPermissionsPartEditViewModel>("ContentPermissionsPart_Edit", model =>
            {
                model.ContentPermissionsPart = part;
                model.Enabled = part.Enabled;
                model.PossibleRoles = roles.ToArray();
                model.Roles = part.Roles;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPermissionsPart model, IUpdateModel updater, UpdatePartEditorContext context)
        {
            await updater.TryUpdateModelAsync(model, Prefix, m => m.Enabled, m => m.Roles);

            if (!model.Enabled)
            {
                model.Roles = Array.Empty<string>();
            }

            return Edit(model, context);
        }

        #endregion
    }
}
