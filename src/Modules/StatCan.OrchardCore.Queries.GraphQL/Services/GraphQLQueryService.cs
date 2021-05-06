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
    public class GraphQLQueryService : IGraphQLQueryService
    {
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly IOptions<GraphQLSettings> _settingsAccessor;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentExecutionListener _dataLoaderDocumentListener;
        private readonly IEnumerable<IValidationRule> _validationRules;
        private readonly ISchemaFactory _schemaService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TemplateOptions _templateOptions;

        public GraphQLQueryService(
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
            _templateOptions = templateOptions.Value;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ExecuteGQLQueryResults> ExecuteQuery(string queryTemplate, IDictionary<string, object> parameters)
        {
            var schema = await _schemaService.GetSchemaAsync();
            var gqlSettings = _settingsAccessor.Value;
            var tokenizedQuery = await _liquidTemplateManager.RenderStringAsync(queryTemplate, NullEncoder.Default, parameters.Select(x => new KeyValuePair<string, FluidValue>(x.Key, FluidValue.Create(x.Value, _templateOptions))));

            var result = new ExecuteGQLQueryResults() { TokenizedQuery = tokenizedQuery };

            result.Result = await _executer.ExecuteAsync(_ =>
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
            return result;
        }
    }
    public class ExecuteGQLQueryResults
    {
        public string TokenizedQuery { get; set; }
        public ExecutionResult Result { get; set; }
    }
}
