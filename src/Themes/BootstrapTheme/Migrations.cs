using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace BootstrapTheme
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
            // feature recipe is executed as a seperate step due to the
            // recipeMigrator not flushing the database session between steps.
            // Any new feature would not be enabled prior to the additional steps running.
            await _recipeMigrator.ExecuteAsync("features.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("initial.recipe.json", this);
            return 1;
        }
    }
}
