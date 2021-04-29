using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Apis.GraphQL;
using OrchardCore.Liquid;
using OrchardCore.Modules;
using StatCan.OrchardCore.Queries.GraphQL.ViewModels;

namespace StatCan.OrchardCore.Queries.GraphQL.Controllers
{
    [Feature("StatCan.OrchardCore.Queries.GraphQL")]
    public class AdminController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILiquidTemplateManager _liquidTemplateManager;
        private readonly IStringLocalizer S;
        private readonly IOptions<GraphQLSettings> _settingsAccessor;
        private readonly IDocumentExecuter _executer;
        private readonly IDocumentExecutionListener _dataLoaderDocumentListener;
        private readonly IEnumerable<IValidationRule> _validationRules;
        private readonly ISchemaFactory _schemaService;
        private readonly TemplateOptions _templateOptions;

        public AdminController(
            IAuthorizationService authorizationService,
            ILiquidTemplateManager liquidTemplateManager,
            IStringLocalizer<AdminController> stringLocalizer,
            IOptions<TemplateOptions> templateOptions,
            IOptions<GraphQLSettings> settingsAccessor,
            IDocumentExecuter executer,
            IDocumentExecutionListener dataLoaderDocumentListener,
            IEnumerable<IValidationRule> validationRules,
            ISchemaFactory schemaService)

        {
            _authorizationService = authorizationService;
            _liquidTemplateManager = liquidTemplateManager;
            S = stringLocalizer;
            _settingsAccessor = settingsAccessor;
            _executer = executer;
            _dataLoaderDocumentListener = dataLoaderDocumentListener;
            _validationRules = validationRules;
            _schemaService = schemaService;
            _templateOptions = templateOptions.Value;
        }

        public Task<IActionResult> Query(string query)
        {
            query = String.IsNullOrWhiteSpace(query) ? "" : System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(query));
            return Query(new AdminQueryViewModel
            {
                DecodedQuery = query,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Query(AdminQueryViewModel model)
        {
            if (!await _authorizationService.AuthorizeAsync(User, Permissions.ManageGraphQLQueries))
            {
                return Forbid();
            }

            if (String.IsNullOrWhiteSpace(model.DecodedQuery))
            {
                return View(model);
            }

            if (String.IsNullOrEmpty(model.Parameters))
            {
                model.Parameters = "{ }";
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var parameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Parameters);

            var tokenizedQuery = await _liquidTemplateManager.RenderStringAsync(model.DecodedQuery, NullEncoder.Default, parameters.Select(x => new KeyValuePair<string, FluidValue>(x.Key, FluidValue.Create(x.Value, _templateOptions))));

            model.RawGraphQL = tokenizedQuery;
            model.Parameters = JsonConvert.SerializeObject(parameters, Formatting.Indented);

            try
            {
                var schema = await _schemaService.GetSchemaAsync();
                var gqlSettings = _settingsAccessor.Value;

                var result = await _executer.ExecuteAsync(_ =>
                {
                    _.Schema = schema;
                    _.Query = tokenizedQuery;
                    _.UserContext = gqlSettings.BuildUserContext?.Invoke(HttpContext);
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

                if(result.Errors?.Count > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Message);
                    }
                }

                var results = new List<JObject>();

                if(result.Data != null)
                {
                    results.Add(JObject.FromObject(result.Data));
                }

                model.Documents = results;
                model.Elapsed = stopwatch.Elapsed;
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", S["An error occurred while executing the GraphQL query: {0}", ex.Message]);
            }

            return View(model);
        }
    }
}
