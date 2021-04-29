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
using Newtonsoft.Json.Linq;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Liquid;
using OrchardCore.Queries;

namespace StatCan.OrchardCore.Queries.GraphQL
{
    public class GraphQLQuerySource : IQuerySource
    {
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly IOptions<GraphQLSettings> _settingsAccessor;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentExecutionListener _dataLoaderDocumentListener;
        private readonly IEnumerable<IValidationRule> _validationRules;
        private readonly ISchemaFactory _schemaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TemplateOptions _templateOptions;

        public GraphQLQuerySource(
            ILiquidTemplateManager liquidTemplateManager,
            IOptions<TemplateOptions> templateOptions,
            IOptions<GraphQLSettings> settingsAccessor,
            IDocumentExecuter executer,
            IDocumentExecutionListener dataLoaderDocumentListener,
            IEnumerable<IValidationRule> validationRules,
            ISchemaFactory schemaService,
            IHttpContextAccessor httpContextAccessor)
        {
            _liquidTemplateManager = liquidTemplateManager;
            _settingsAccessor = settingsAccessor;
            _executer = executer;
            _dataLoaderDocumentListener = dataLoaderDocumentListener;
            _validationRules = validationRules;
            _schemaService = schemaService;
            _httpContextAccessor = httpContextAccessor;
            _templateOptions = templateOptions.Value;
        }

        public string Name => "GraphQL";

        public Query Create()
        {
            return new GraphQLQuery();
        }

        public async Task<IQueryResults> ExecuteQueryAsync(Query query, IDictionary<string, object> parameters)
        {
            var graphQLQuery = query as GraphQLQuery;
            var graphQLQueryResults = new GraphQLQueryResults();

            var tokenizedQuery = await _liquidTemplateManager.RenderStringAsync(graphQLQuery.Template, NullEncoder.Default,
                parameters.Select(x => new KeyValuePair<string, FluidValue>(x.Key, FluidValue.Create(x.Value, _templateOptions))));

                var schema = await _schemaService.GetSchemaAsync();
                var gqlSettings = _settingsAccessor.Value;

                var result = await _executer.ExecuteAsync(_ =>
                {
                    _.Schema = schema;
                    _.Query = tokenizedQuery;
                    _.UserContext = gqlSettings.BuildUserContext?.Invoke(_httpContextAccessor.HttpContext);
                    _.ExposeExceptions = gqlSettings.ExposeExceptions;
                    _.ValidationRules = DocumentValidator.CoreRules()
                                        .Concat(_validationRules);
                    _.ComplexityConfiguration = new ComplexityConfiguration
                    {
                        MaxDepth = gqlSettings.MaxDepth,
                        MaxComplexity = gqlSettings.MaxComplexity,
                        FieldImpact = gqlSettings.FieldImpact
                    };
                    _.Listeners.Add(_dataLoaderDocumentListener);
                });

                graphQLQueryResults.Items = new List<JObject>{};

                var results = new List<JObject>();

                if(result.Data != null)
                {
                    results.Add(JObject.FromObject(result.Data));
                }

                graphQLQueryResults.Items = results;
                // Too bad the return value must be a list.
                return graphQLQueryResults;
        }
    }
}
