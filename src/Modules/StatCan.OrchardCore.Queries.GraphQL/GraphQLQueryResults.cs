using System.Collections.Generic;
using OrchardCore.Queries;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    public class GraphQLQueryResults : IQueryResults
    {
        public IEnumerable<object> Items { get; set; }
    }
}
