using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;

namespace StatCan.OrchardCore.WebpageCore
{
    public class FatFooterMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;


        public FatFooterMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("FatFooter.recipe.json", this);
            return 1;
        }
    }
    public class HeroMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;


        public HeroMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("Hero.recipe.json", this);
            return 1;
        }
    }
    public class PageMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;


        public PageMigration(IRecipeMigrator recipeMigrator)
        {
            _recipeMigrator = recipeMigrator;
        }

        public async Task<int> CreateAsync()
        {
            await _recipeMigrator.ExecuteAsync("Page.recipe.json", this);
            return 1;
        }
    }
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

    public class ShowcaseBlurbMigration : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;


        public ShowcaseBlurbMigration(IRecipeMigrator recipeMigrator)
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
