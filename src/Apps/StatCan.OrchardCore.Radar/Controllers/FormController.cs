using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using OrchardCore.ContentManagement.Display;
using OrchardCore.DisplayManagement.ModelBinding;
using Etch.OrchardCore.ContentPermissions.Services;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Contents;
using OrchardCore.Shortcodes.Services;
using OrchardCore.Taxonomies.Models;
using StatCan.OrchardCore.Radar.Services;

using Permissions = OrchardCore.Contents.Permissions;

namespace StatCan.OrchardCore.Radar.Controllers
{
    public class FormController : Controller
    {
        private readonly IContentManager _contentManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IQueryManager _queryManager;
        private readonly IUpdateModelAccessor _updateModelAccessor;
        private readonly IShortcodeService _shortcodeService;
        private IContentItemDisplayManager _contentItemDisplayManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IContentPermissionsService _contentPermissionsService;
        private readonly TaxonomyManager _taxonomyManager;

        private readonly BagItemManager _bagItemManager;

        public FormController(IContentManager contentManager, IHttpContextAccessor httpContextAccessor,
            IQueryManager queryManager, IContentItemDisplayManager contentItemDisplayManager,
            IUpdateModelAccessor updateModelAccessor, IShortcodeService shortcodeService, IAuthorizationService authorizationService,
            IContentPermissionsService contentPermissionsService, TaxonomyManager taxonomyManager, BagItemManager bagItemManager)
        {
            _contentManager = contentManager;
            _httpContextAccessor = httpContextAccessor;
            _queryManager = queryManager;
            _contentItemDisplayManager = contentItemDisplayManager;
            _updateModelAccessor = updateModelAccessor;
            _shortcodeService = shortcodeService;
            _authorizationService = authorizationService;
            _contentPermissionsService = contentPermissionsService;
            _taxonomyManager = taxonomyManager;
            _bagItemManager = bagItemManager;
        }

        public async Task<IActionResult> Form(string entityType, string id)
        {
            // Builds the form shape. It is assumed that there
            // will ever be 1 form content item
            var form = await GetFormAsync(GetFormNameFromType(entityType));

            if (form == null)
            {
                return Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found");
            }


            var formShape = await _contentItemDisplayManager.BuildDisplayAsync(form, _updateModelAccessor.ModelUpdater, "Detail");

            return View(formShape);
        }

        // For handling contents that are contained by other contents
        public async Task<IActionResult> FormContained(string parentType, string parentId, string childType, string id)
        {
            if (!await CanEditContent(parentId))
            {
                return Redirect("not-found");
            }

            // Builds the form shape. It is assumed that there
            // will ever be 1 form content item
            var form = await GetFormAsync(GetFormNameFromType(childType));

            if (form == null)
            {
                return Redirect($"{_httpContextAccessor.HttpContext.Request.PathBase}/not-found");
            }


            var formShape = await _contentItemDisplayManager.BuildDisplayAsync(form, _updateModelAccessor.ModelUpdater, "Detail");

            return View("Form", formShape);
        }

        // Api actions for forms
        public async Task<IActionResult> TopicSearch(string term)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            ICollection<IDictionary<string, string>> topics = new LinkedList<IDictionary<string, string>>();

            if (term != null)
            {
                // Each topic needs to be retrived from the taxonomy term
                var topicQuery = await _queryManager.GetQueryAsync("AllTaxonomiesSQL");
                var topicResult = await _queryManager.ExecuteQueryAsync(topicQuery, new Dictionary<string, object> { { "type", "Topics" } });

                if (topicResult != null)
                {
                    var topicPart = (topicResult.Items.First() as ContentItem).As<TaxonomyPart>();

                    foreach (var topic in topicPart.Terms)
                    {
                        if (!_contentPermissionsService.CanAccess(topic))
                        {
                            continue;
                        }

                        // Delocalize the topic name
                        var topicName = await _shortcodeService.ProcessAsync(topic.DisplayText);

                        if (topicName.Contains(term, StringComparison.OrdinalIgnoreCase))
                        {
                            var valuePair = new Dictionary<string, string>()
                        {
                            {"value", topic.ContentItemId},
                            {"label", topicName}
                        };

                            topics.Add(valuePair);
                        }
                    }
                }
            }

            return new ObjectResult(topics);
        }

        public async Task<IActionResult> UserSearch(string term)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            ICollection<IDictionary<string, string>> users = new LinkedList<IDictionary<string, string>>();

            // Each topic needs to be retrived from the taxonomy term
            var userQuery = await _queryManager.GetQueryAsync("AllUsersSQL");
            var userResult = await _queryManager.ExecuteQueryAsync(userQuery, new Dictionary<string, object>());

            if (term != null)
            {
                if (userResult != null)
                {
                    foreach (JObject user in userResult.Items)
                    {
                        var userName = user["NormalizedUserName"].Value<string>().ToLower();

                        if (userName.Contains(term, StringComparison.OrdinalIgnoreCase))
                        {
                            var optionPair = new Dictionary<string, string>()
                            {
                                {"value", user["UserId"].Value<string>() },
                                {"label", userName}
                            };

                            users.Add(optionPair);
                        }
                    }
                }
            }

            return new ObjectResult(users);
        }

        public async Task<IActionResult> EntitySearch(string type, string term)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            // Get the lucene query
            var query = await _queryManager.GetQueryAsync("EntityListLucene");

            // Prepare the parameters
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("Type", type);
            parameters.Add("Term", term != null ? term.ToLower() : "");

            var results = await _queryManager.ExecuteQueryAsync(query, parameters);

            // Convert result to content items
            ICollection<IDictionary<string, string>> entities = new LinkedList<IDictionary<string, string>>();

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
                    else if (part == null && part.Culture != CultureInfo.CurrentCulture.Name)
                    {
                        continue;
                    }

                    var optionPair = new Dictionary<string, string>()
                    {
                        {"value", part.LocalizationSet},
                        {"label", contentItem.DisplayText}
                    };

                    entities.Add(optionPair);
                }
            }

            return new ObjectResult(entities);
        }


        [HttpPost]
        public async Task<IActionResult> ContentDelete([FromForm] string entityType, [FromForm] string id, [FromForm] string parentId, [FromForm] string successUrl, [FromForm] string failUrl)
        {
            if (entityType == "Topic")
            {
                if (!IsUserAdmin())
                {
                    return Redirect(failUrl);
                }

                await _taxonomyManager.DeleteTaxonomyAsync("Topics", id);

                return Redirect(successUrl);
            }

            if (entityType == "Artifact")
            {
                if (!await CanEditContent(parentId) || !await _authorizationService.AuthorizeAsync(User, Permissions.DeleteContent))
                {
                    return Redirect(failUrl);
                }

                await _bagItemManager.DeleteBagItemAsync("Workspace", parentId, id);
            }

            if (!await CanEditContent(id) || !await _authorizationService.AuthorizeAsync(User, Permissions.DeleteContent))
            {
                return Redirect(failUrl);
            }

            var contentItem = await _contentManager.GetAsync(id);

            await _contentManager.RemoveAsync(contentItem);

            return Redirect(successUrl);
        }

        private async Task<bool> CanEditContent(string id)
        {
            return IsUserAdmin() || await IsOwner(id);
        }

        private bool IsUserAdmin()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user == null)
            {
                return false;
            }

            if (user.IsInRole("Administrator"))
            {
                return true;
            }

            return false;
        }

        private async Task<bool> IsOwner(string id)
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user == null)
            {
                return false;
            }

            var contentItem = await _contentManager.GetAsync(id);

            if (contentItem == null)
            {
                return false;
            }

            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            return contentItem.Owner == userId;
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

        private string GetFormNameFromType(string type)
        {
            if (type == "topics")
            {
                return "Topic Form";
            }
            else if (type == "artifacts")
            {
                return "Artifact Form";
            }
            else if (type == "projects")
            {
                return "Project Form";
            }
            else if (type == "communities")
            {
                return "Community Form";
            }
            else if (type == "events")
            {
                return "Event Form";
            }
            else if (type == "proposals")
            {
                return "Proposal Form";
            }

            return "";
        }
    }
}
