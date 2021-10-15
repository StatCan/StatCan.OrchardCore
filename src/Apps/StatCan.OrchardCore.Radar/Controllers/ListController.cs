using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using Etch.OrchardCore.ContentPermissions.Services;

namespace StatCan.OrchardCore.Radar.Controllers
{
    public class ListController : Controller
    {
        private readonly IQueryManager _queryManager;
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IContentPermissionsService _contentPermissionsService;

        private const string LIST_QUERY = "EntityListLucene";

        public ListController(IQueryManager queryManager, IContentItemDisplayManager contentItemDisplayManager, IUpdateModelAccessor updateModelAccessor, IContentPermissionsService contentPermissionsService)
        {
            _queryManager = queryManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _contentPermissionsService = contentPermissionsService;
        }

        [HttpGet]
        [Route("/proposals")]
        public async Task<IActionResult> Proposals(string searchText = null)
        {
            return View(await GetContents("Proposal", searchText));
        }

        [HttpGet]
        [Route("/projects")]
        public async Task<IActionResult> Projects(string searchText = null)
        {
            return View(await GetContents("Project", searchText));
        }

        [HttpGet]
        [Route("/events")]
        public async Task<IActionResult> Events(string searchText = null)
        {
            return View(await GetContents("Event", searchText));
        }

        [HttpGet]
        [Route("/communities")]
        public async Task<IActionResult> Communities(string searchText = null)
        {
            return View(await GetContents("Community", searchText));
        }

        [HttpPost]
        public IActionResult Search([FromForm] string type, [FromForm] string searchText)
        {
            return RedirectToAction(type, new { searchText = searchText });
        }

        [HttpGet]
        public async Task<IActionResult> GlobalSearch(string searchText = "")
        {
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

            if(results != null)
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

                    // If input is a 'JObject' but which not represents a 'ContentItem',
                    // a 'ContentItem' is still created but with some null properties.
                    if (contentItem?.ContentItemId == null)
                    {
                        continue;
                    }
                    // Permission check
                    else if(!_contentPermissionsService.CanAccess(contentItem))
                    {
                        continue;
                    }

                    contentItems.Add(contentItem);
                }
            }

            return contentItems;
        }
    }
}
