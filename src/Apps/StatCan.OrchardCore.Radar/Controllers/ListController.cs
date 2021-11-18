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

namespace StatCan.OrchardCore.Radar.Controllers
{
    public class ListController : Controller
    {
        private readonly IQueryManager _queryManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IContentPermissionsService _contentPermissionsService;
        private readonly IAuthorizationService _authorizationService;

        private const string LIST_QUERY = "EntityListLucene";

        public ListController(
            IQueryManager queryManager,
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IContentPermissionsService contentPermissionsService,
            IAuthorizationService authorizationService)
        {
            _queryManager = queryManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _contentPermissionsService = contentPermissionsService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> List(string searchText = null)
        {
            if (!await CanViewList())
            {
                return Redirect("not-found");
            }

            var type = (string)RouteData.DataTokens["type"];

            return View(type, await GetContents(type, searchText));
        }

        [HttpPost]
        public IActionResult Search([FromForm] string type, [FromForm] string searchText)
        {
            return RedirectToRoute(type + "ListView", new { searchText = searchText });
        }

        [HttpGet]
        public async Task<IActionResult> GlobalSearch(string searchText = "")
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var results = await GetContentItems("*", searchText);

            return new ObjectResult(results);
        }

        private async Task<IEnumerable<dynamic>> GetShapes(IEnumerable<ContentItem> contentItems)
        {
            var shapes = await Task.WhenAll(contentItems.Select(async contentItem =>
            {
                return await _contentItemDisplayManager.BuildDisplayAsync(contentItem, _updateModelAccessor.ModelUpdater, "Summary");
            }));

            return shapes;
        }

        private async Task<IEnumerable<dynamic>> GetContents(string type, string searchText)
        {
            if (String.IsNullOrWhiteSpace(searchText))
            {
                searchText = "*";
                ViewData["searchText"] = "";
            }
            else
            {
                ViewData["searchText"] = searchText;
            }

            var contentItems = await GetContentItems(type, searchText);
            return await GetShapes(contentItems);
        }
        private async Task<IEnumerable<ContentItem>> GetContentItems(string type, string searchText)
        {
            // Get the lucene query
            var query = await _queryManager.GetQueryAsync(LIST_QUERY);

            // Prepare the parameters
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Type", type);
            parameters.Add("Term", searchText != null ? searchText.ToLower() : "");

            var results = await _queryManager.ExecuteQueryAsync(query, parameters);

            // Convert result to content items
            var contentItems = new List<ContentItem>();

            if (results != null)
            {
                foreach (var result in results.Items)
                {
                    if (!(result is ContentItem contentItem))
                    {
                        contentItem = null;

                        if (result is JObject jObject)
                        {
                            contentItem = jObject.ToObject<ContentItem>();
                        }
                    }

                    var part = contentItem.As<LocalizationPart>();

                    // If input is a 'JObject' but which not represents a 'ContentItem',
                    // a 'ContentItem' is still created but with some null properties.
                    if (contentItem?.ContentItemId == null)
                    {
                        continue;
                    }
                    // Orchard content permission check
                    else if (!await _authorizationService.AuthorizeAsync(User, CommonPermissions.ViewContent, contentItem))
                    {
                        continue;
                    }
                    // Content Permission check
                    else if (!_contentPermissionsService.CanAccess(contentItem))
                    {
                        continue;
                    }
                    // Culture check
                    else if (part != null && part.Culture != CultureInfo.CurrentCulture.Name)
                    {
                        continue;
                    }

                    contentItems.Add(contentItem);
                }
            }

            return contentItems;
        }

        private async Task<Boolean> CanViewList()
        {
            if (!await _authorizationService.AuthorizeAsync(User, CommonPermissions.ListContent))
            {
                return false;
            }

            return true;
        }
    }
}
