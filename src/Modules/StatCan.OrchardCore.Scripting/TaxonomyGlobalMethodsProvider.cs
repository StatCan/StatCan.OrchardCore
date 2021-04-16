using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Scripting;
using YesSql;
using OrchardCore.Taxonomies;

namespace StatCan.OrchardCore.Scripting
{
    public class TaxonomyGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _taxonomyTerms;
        private readonly GlobalMethod _taxonomyTerms2;

        public TaxonomyGlobalMethodsProvider()
        {
            _taxonomyTerms = new GlobalMethod
            {
                Name = "taxonomyTerms",
                Method = serviceProvider => (Func<string, string[], IEnumerable<ContentItem>>)((taxonomyContentItemId, termContentItemIds) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();

                    var taxonomy = contentManager.GetAsync(taxonomyContentItemId).GetAwaiter().GetResult();

                    if(taxonomy == null)
                    {
                        return null;
                    }

                    var terms = new List<ContentItem>();
                    foreach (var termContentItemId in termContentItemIds)
                    {
                        var term = FindTerm(taxonomy.Content.TaxonomyPart.Terms as JArray, termContentItemId);

                        if (term != null)
                        {
                            terms.Add(term);
                        }
                    }
                    return terms;
                }
                )
            };
             _taxonomyTerms2 = new GlobalMethod
            {
                Name = "taxonomyTermsJobject",
                Method = serviceProvider => (Func<JObject, IEnumerable<ContentItem>>)((termObject) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    var taxonomyContentItemId = termObject["TaxonomyContentItemId"].Value<string>();
                    var termContentItemIds = ((JArray)termObject["TermContentItemIds"]).Values<string>().ToArray();

                    var taxonomy = contentManager.GetAsync(taxonomyContentItemId).GetAwaiter().GetResult();

                    if(taxonomy == null)
                    {
                        return null;
                    }

                    var terms = new List<ContentItem>();
                    foreach (var termContentItemId in termContentItemIds)
                    {
                        var term = FindTerm(taxonomy.Content.TaxonomyPart.Terms as JArray, termContentItemId);

                        if (term != null)
                        {
                            terms.Add(term);
                        }
                    }
                    return terms;
                }
                )
            };
        }
        private static ContentItem FindTerm(JArray termsArray, string termContentItemId)
        {
            foreach (JObject term in termsArray)
            {
                var contentItemId = term.GetValue("ContentItemId").ToString();

                if (contentItemId == termContentItemId)
                {
                    return term.ToObject<ContentItem>();
                }

                if (term.GetValue("Terms") is JArray children)
                {
                    var found = FindTerm(children, termContentItemId);

                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _taxonomyTerms, _taxonomyTerms2 };
        }
    }
}
