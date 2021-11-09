using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using Etch.OrchardCore.ContentPermissions.Services;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Contents;
using OrchardCore.Taxonomies.Models;

namespace StatCan.OrchardCore.Radar.Controllers
{
    public class FormController : Controller
    {
        private IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryManager _queryManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;

        private IContentItemDisplayManager _contentItemDisplayManager;

        public FormController(IContentManager contentManager, IHttpContextAccessor httpContextAccessor, IQueryManager queryManager, IContentItemDisplayManager contentItemDisplayManager, IUpdateModelAccessor updateModelAccessor)
        {
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;
            _queryManager = queryManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
        }

        public async Task<IActionResult> TopicForm(string id)
        {
            // Builds the form shape. It is assumed that there
            // will ever be 1 form content item
            var form = await GetFormAsync("Topic Form");

            if (form == null)
            {
                return Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found");
            }


            var formShape = await _contentItemDisplayManager.BuildDisplayAsync(form, _updateModelAccessor.ModelUpdater, "Detail");

            return View(formShape);
        }

        private async Task<ContentItem> GetFormAsync(string formName)
        {
            var query = await _queryManager.GetQueryAsync("FormQuerySQL");
            var result = await _queryManager.ExecuteQueryAsync(query, new Dictionary<string, object> { { "type", formName } });

            if (result == null)
            {
                return null;
            }

            var id = (result.Items.First() as JObject).GetValue("ContentItemId").ToString();

            var contentItem = await _contentManager.GetAsync(id);

            return contentItem;
        }
    }
}
