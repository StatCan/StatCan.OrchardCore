using System.Threading.Tasks;
using OrchardCore.Recipes.Services;
using OrchardCore.Data.Migration;

namespace StatCan.OrchardCore.Radar.Migrations
{
    public class AppMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public AppMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("init.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("app.recipe.json", this);

            return 1;
        }
    }
}
