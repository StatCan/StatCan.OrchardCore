using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etch.OrchardCore.Blocks.Services
{
    public interface IContentSearchResultsProvider
    {
        Task<IEnumerable<ContentSearchResult>> SearchAsync(ContentSearchContext searchContext);
    }

    public class ContentSearchContext
    {
        public string Query { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }
    }

    public class ContentSearchResult
    {
        public string DisplayText { get; set; }
        public string ContentItemId { get; set; }
        public bool HasPublished { get; set; }
        public string Url { get; set; }
    }
}
