using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.WebpageCore
{
    public class PageLayoutMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public PageLayoutMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("Hero.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("Section.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("FatFooter.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("Page.recipe.json", this);
            return 1;
        }
    }

    public class ContentLayoutMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public ContentLayoutMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("ShowcaseBlurb.recipe.json", this);
            return 1;
        }
    }

    public class MenuItemPartsMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;

        public MenuItemPartsMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("MenuItemParts.recipe.json", this);
            return 1;
        }
    }
}
