using Microsoft.AspNetCore.Mvc.Routing;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YesSql;
using YesSql.Services;

namespace Etch.OrchardCore.Blocks.Services
{
    public class DefaultContentSearchResultsProvider : IContentSearchResultsProvider
    {
        #region Constants

        private const int SearchResultCount = 10;

        #endregion

        #region Dependencies

        private readonly IContentManager _contentManager;
        private readonly ISession _session;
        private readonly IUrlHelperFactory _urlHelperFactory;

        #endregion

        #region Constructor

        public DefaultContentSearchResultsProvider(IContentManager contentManager, ISession session, IUrlHelperFactory urlHelperFactory)
        {
            _contentManager = contentManager;
            _session = session;
            _urlHelperFactory = urlHelperFactory;
        }

        #endregion

        #region Implementation

        public async Task<IEnumerable<ContentSearchResult>> SearchAsync(ContentSearchContext searchContext)
        {
            var query = _session.Query<ContentItem, ContentItemIndex>()
                .With<ContentItemIndex>(x => x.ContentType.IsIn(searchContext.ContentTypes) && x.Latest);

            if (!string.IsNullOrEmpty(searchContext.Query))
            {
                query.With<ContentItemIndex>(x => x.DisplayText.Contains(searchContext.Query) || x.ContentType.Contains(searchContext.Query));
            }

            var contentItems = await query.Take(SearchResultCount).ListAsync();
            var results = new List<ContentSearchResult>();


            foreach (var contentItem in contentItems)
            {
                var metadata = await _contentManager.PopulateAspectAsync<ContentItemMetadata>(contentItem);


                results.Add(new ContentSearchResult
                {
                    ContentItemId = contentItem.ContentItemId,
                    DisplayText = contentItem.ToString(),
                    HasPublished = await _contentManager.HasPublishedVersionAsync(contentItem),
                    Url = await GetDisplayUrlAsync(contentItem)
                });
            }

            return results.OrderBy(x => x.DisplayText);
        }

        #endregion

        #region Helper Methods

        private async Task<string> GetDisplayUrlAsync(ContentItem contentItem)
        {
            if (contentItem.Has<AutoroutePart>())
            {
                return $"/{contentItem.As<AutoroutePart>().Path}";
            }

            var metadata = await _contentManager.PopulateAspectAsync<ContentItemMetadata>(contentItem);

            return string.Join("/", metadata.DisplayRouteValues.Select(x => x.Value).ToArray());
        }

        #endregion
    }
}
