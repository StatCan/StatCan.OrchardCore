using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using OrchardCore.Queries;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Models;

namespace StatCan.OrchardCore.Radar.Services
{
    public class TaxonomyManager
    {
        private readonly IQueryManager _queryManager;
        private const string TAXONOMY_QUERY = "AllTaxonomiesSQL";

        public TaxonomyManager(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }

        public async Task<ContentItem> GetTaxonomyTermByIdAsync(string type, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            // Each topic needs to be retrived from the taxonomy term
            var terms = await GetTaxonomyTermsAsync(type);

            if (terms != null)
            {
                foreach (var term in terms)
                {
                    if (id.Equals(term.ContentItemId))
                    {
                        return term;
                    }
                }
            }

            return null;
        }

        public async Task<ICollection<ContentItem>> GetTaxonomyTermsAsync(string type)
        {
            // Each topic needs to be retrived from the taxonomy term
            var taxonomyQuery = await _queryManager.GetQueryAsync(TAXONOMY_QUERY);
            var taxonomyResult = await _queryManager.ExecuteQueryAsync(taxonomyQuery, new Dictionary<string, object> { { "type", type } });

            if (taxonomyResult != null)
            {
                var taxonomy = taxonomyResult.Items.First() as ContentItem;
                var part = taxonomy.As<TaxonomyPart>();

                return part.Terms;
            }

            return null;
        }
    }
}
