using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.Scheduling
{
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public Migrations(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("initial.recipe.json", this);
            return 1;
        }
    }
}
