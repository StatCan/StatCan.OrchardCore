using OrchardCore.Queries;

namespace StatCan.OrchardCore.Radar.Services
{
    public class BaseContentConverterDependency
    {
        private readonly IQueryManager _queryManager;

        public BaseContentConverterDependency(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }

        public IQueryManager GetQueryManager()
        {
            return _queryManager;
        }
    }
}
