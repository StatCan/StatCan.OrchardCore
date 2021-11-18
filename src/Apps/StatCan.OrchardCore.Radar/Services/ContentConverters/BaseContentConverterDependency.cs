using OrchardCore.ContentManagement;
using OrchardCore.Queries;

namespace StatCan.OrchardCore.Radar.Services
{
    public class BaseContentConverterDependency
    {
        private readonly IQueryManager _queryManager;
        private readonly IContentManager _contentManager;

        public BaseContentConverterDependency(IQueryManager queryManager, IContentManager contentManager)
        {
            _queryManager = queryManager;
            _contentManager = contentManager;
        }

        public IQueryManager GetQueryManager()
        {
            return _queryManager;
        }

        public IContentManager GetContentManager()
        {
            return _contentManager;
        }
    }
}
