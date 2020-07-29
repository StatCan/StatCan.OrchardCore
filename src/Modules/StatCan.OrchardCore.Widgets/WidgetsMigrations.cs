using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.Widgets
{
    public class SectionMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;


        public SectionMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("Section.recipe.json", this);
            return 1;
        }
    }
}
