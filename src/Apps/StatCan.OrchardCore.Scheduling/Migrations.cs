using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.Scheduling
{
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager contentDefinitionManager)
        {
            _recipeMigrator = recipeMigrator;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("initial.recipe.json", this);
            _contentDefinitionManager.AlterTypeDefinition("Taxonomy", t=>t.Securable());
            return 2;
        }
        public async Task<int> UpdateFrom1Async()
        {
            // re-run the recipe for existing deployments
            await _recipeMigrator.ExecuteAsync("initial.recipe.json", this);
            _contentDefinitionManager.AlterTypeDefinition("Taxonomy", t=>t.Securable());
            return 2;
        }
    }
}
