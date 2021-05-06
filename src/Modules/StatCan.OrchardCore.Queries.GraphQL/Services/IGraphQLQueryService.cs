using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.Queries.GraphQL.Services
{
    public interface IGraphQLQueryService
    {
        Task<ExecuteGQLQueryResults> ExecuteQuery(string queryTemplate, IDictionary<string, object> parameters);
    }
}
