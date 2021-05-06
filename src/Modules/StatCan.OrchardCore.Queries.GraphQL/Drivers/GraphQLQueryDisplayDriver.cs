using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Queries;
using StatCan.OrchardCore.Queries.GraphQL.ViewModels;

namespace StatCan.OrchardCore.Queries.GraphQL.Drivers
{
    public class GraphQLQueryDisplayDriver : DisplayDriver<Query, GraphQLQuery>
    {
        private readonly IStringLocalizer S;

        public GraphQLQueryDisplayDriver(IStringLocalizer<GraphQLQueryDisplayDriver> stringLocalizer)
        {
            S = stringLocalizer;
        }

        public override IDisplayResult Display(GraphQLQuery query, IUpdateModel updater)
        {
            return Combine(
                Dynamic("GraphQLQuery_SummaryAdmin", model =>
                {
                    model.Query = query;
                }).Location("Content:5"),
                Dynamic("GraphQLQuery_Buttons_SummaryAdmin", model =>
                {
                    model.Query = query;
                }).Location("Actions:2")
            );
        }

        public override IDisplayResult Edit(GraphQLQuery query, IUpdateModel updater)
        {
            return Initialize<GraphQLQueryViewModel>("GraphQLQuery_Edit", model =>
            {
                model.Query = query.Template;

                // Extract query from the query string if we come from the main query editor
                if (string.IsNullOrEmpty(query.Template))
                {
                    updater.TryUpdateModelAsync(model, "", m => m.Query);
                }
            }).Location("Content:5");
        }

        public override async Task<IDisplayResult> UpdateAsync(GraphQLQuery model, IUpdateModel updater)
        {
            var viewModel = new GraphQLQueryViewModel();
            if (await updater.TryUpdateModelAsync(viewModel, Prefix, m => m.Query))
            {
                model.Template = viewModel.Query;
            }

            if (String.IsNullOrWhiteSpace(model.Template))
            {
                updater.ModelState.AddModelError(nameof(viewModel.Query), S["The query field is required"]);
            }

            return Edit(model, updater);
        }
    }
}
