using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Contents;
using StatCan.OrchardCore.Radar.Services;

namespace StatCan.OrchardCore.Radar.Controllers
{
    
        /*
            Builds list views and handles list search.
        */
    public class ListController : Controller
    {
        private readonly IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IAuthorizationService _authorizationService;
        private readonly EntitySearcher _entitySearcher;

        public ListController(
            IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor,
            IAuthorizationService authorizationService,
            EntitySearcher entitySearcher)
        {
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _authorizationService = authorizationService;
            _entitySearcher = entitySearcher;
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

            var results = await _entitySearcher.SearchContentItemsAsync("*", searchText);

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

            var contentItems = await _entitySearcher.SearchContentItemsAsync(type, searchText);
            return await GetShapes(contentItems);
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
