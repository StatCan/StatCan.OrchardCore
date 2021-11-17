using OrchardCore.Queries;

namespace StatCan.OrchardCore.Radar.Services
{
    public class BaseContentConverterDenpency
    {
        private readonly IQueryManager _queryManager;

        public BaseContentConverterDenpency(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }

        public IQueryManager GetQueryManager()
        {
            return _queryManager;
        }
    }
}
