using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    // Returns all terms of a taxonomy, including nested terms
    public class AllTaxonomyTermsFilter : ILiquidFilter
    {
        private readonly IContentManager _contentManager;

        public AllTaxonomyTermsFilter(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext ctx)
        {
            string taxonomyContentItemId = input.ToStringValue();

            var taxonomy = await _contentManager.GetAsync(taxonomyContentItemId);

            if (taxonomy == null)
            {
                return null;
            }

            var terms = GetTermsRecursive(taxonomy.Content.TaxonomyPart.Terms as JArray);

            return FluidValue.Create(terms, ctx.Options);
        }

        private static IEnumerable<ContentItem> GetTermsRecursive(JArray termsArray)
        {
            var terms = new List<ContentItem>();
            foreach (JObject term in termsArray)
            {
                terms.Add(term.ToObject<ContentItem>());

                if (term.GetValue("Terms") is JArray children)
                {
                    terms.AddRange(GetTermsRecursive(children));
                }
            }
            return terms;
        }
    }
}
