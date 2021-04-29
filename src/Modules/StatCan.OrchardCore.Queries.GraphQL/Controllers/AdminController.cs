using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Modules;
using StatCan.OrchardCore.Queries.GraphQL.Services;
using StatCan.OrchardCore.Queries.GraphQL.ViewModels;

namespace StatCan.OrchardCore.Queries.GraphQL.Controllers
{
    [Feature("StatCan.OrchardCore.Queries.GraphQL")]
    public class AdminController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IStringLocalizer S;
        private readonly IGraphQLQueryService _queryService;

        public AdminController(
            IAuthorizationService authorizationService,
            IStringLocalizer<AdminController> stringLocalizer,
            IGraphQLQueryService queryService)

        {
            _authorizationService = authorizationService;
            S = stringLocalizer;
            _queryService = queryService;
        }

        public Task<IActionResult> Query(string query)
        {
            query = String.IsNullOrWhiteSpace(query) ? "" : System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(query));

            // this executes the query if any
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

            model.Parameters = JsonConvert.SerializeObject(parameters, Formatting.Indented);

            try
            {
                var gqlQueryResult = await _queryService.ExecuteQuery(model.DecodedQuery, parameters);
                model.RawGraphQL = gqlQueryResult.TokenizedQuery;

                if(gqlQueryResult.Result.Errors?.Count > 0)
                {
                    foreach (var error in gqlQueryResult.Result.Errors)
                    {
                        ModelState.AddModelError("", error.Message);
                    }
                }
                model.Documents =  new List<JObject>
                {
                    JObject.FromObject(gqlQueryResult.Result)
                };
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
