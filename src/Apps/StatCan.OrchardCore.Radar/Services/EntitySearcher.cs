using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using OrchardCore.ContentManagement;
using OrchardCore.Queries;
using Etch.OrchardCore.ContentPermissions.Services;
using OrchardCore.ContentLocalization.Models;
using OrchardCore.Contents;
using OrchardCore.Shortcodes.Services;
using StatCan.OrchardCore.Radar.Models;
using StatCan.OrchardCore.Radar.Helpers;
using StatCan.OrchardCore.Radar.Services;

namespace StatCan.OrchardCore.Radar.Services
{
    public class EntitySearcher
    {
        private readonly IQueryManager _queryManager;
        private readonly IContentPermissionsService _contentPermissionsService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShortcodeService _shortcodeService;
        private readonly TaxonomyManager _taxonomyManager;

        private const string LIST_QUERY = "EntityListLucene";

        public EntitySearcher(
            IQueryManager queryManager,
            IContentPermissionsService contentPermissionsService,
            IAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,
            TaxonomyManager taxonomyManager,
            IShortcodeService shortcodeService)
        {
            _queryManager = queryManager;
            _contentPermissionsService = contentPermissionsService;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _shortcodeService = shortcodeService;
            _taxonomyManager = taxonomyManager;
        }

        public async Task<ICollection<ContentItem>> SearchTaxonomyAsync(string type, string searchText)
        {
            var terms = await _taxonomyManager.GetTaxonomyTermsAsync(type);
            ICollection<ContentItem> contentItems = new LinkedList<ContentItem>();

            foreach (var term in terms)
            {
                if (!_contentPermissionsService.CanAccess(term))
                {
                    continue;
                }

                // Delocalize the term name
                var termName = await _shortcodeService.ProcessAsync(term.DisplayText);

                if (termName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    contentItems.Add(term);
                }
            }

            return contentItems;
        }

        public async Task<ICollection<ContentItem>> SearchContentItemsAsync(string type, string searchText)
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

                    var localizationPart = contentItem.As<LocalizationPart>();
                    var radarPermissionPart = contentItem.As<RadarPermissionPart>();
                    var user = _httpContextAccessor.HttpContext.User;

                    if (contentItem?.ContentItemId == null || user == null)
                    {
                        continue;
                    }
                    // Orchard content permission check
                    else if (!await _authorizationService.AuthorizeAsync(user, CommonPermissions.ViewContent, contentItem))
                    {
                        continue;
                    }
                    // Content Permission check
                    else if (!_contentPermissionsService.CanAccess(contentItem))
                    {
                        continue;
                    }
                    // Culture check
                    else if (localizationPart != null && localizationPart.Culture != CultureInfo.CurrentCulture.Name)
                    {
                        continue;
                    }
                    // Publish/Draft check
                    else if (!radarPermissionPart.Published && !Ownership.IsOwner(contentItem, user))
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
