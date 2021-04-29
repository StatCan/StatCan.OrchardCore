using OrchardCore.Queries;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    public class GraphQLQuery : Query
    {
        public GraphQLQuery() : base("GraphQL")
        {
        }

        public string Template { get; set; }
    }
}
