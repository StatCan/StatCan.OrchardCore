
using System.Threading.Tasks;
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Themes.Services;
using StatCan.OrchardCore.Vuetify;

namespace StatCan.Themes.HackathonTheme
{
    public class Migrations : DataMigration
    {
        private readonly ISiteThemeService _siteThemeService;
        private readonly IExtensionManager _extensionManager;
        private readonly IShellFeaturesManager _shellFeaturesManager;

        public Migrations(ISiteThemeService siteThemeService,
            IExtensionManager extensionManager,
            IShellFeaturesManager shellFeaturesManager)
        {
            _siteThemeService = siteThemeService;
            _extensionManager = extensionManager;
            _shellFeaturesManager = shellFeaturesManager;
        }

        public async Task<int> CreateAsync()
        {
            await MigrateToVuetifyTheme();
            return 8;
        }

        public int UpdateFrom1()
        {
            return 2;
        }

        public int UpdateFrom2()
        {
            return 3;
        }

        public int UpdateFrom3()
        {
            return 4;
        }

        public int UpdateFrom4()
        {
            return 5;
        }

        public int UpdateFrom5()
        {
            return 6;
        }

        public int UpdateFrom6()
        {
            return 7;
        }
        public async Task<int> UpdateFrom7()
        {
            await MigrateToVuetifyTheme();
            return 8;
        }

        private async Task MigrateToVuetifyTheme()
        {
            var vuetifyModule = _extensionManager.GetFeatures(new []{
                "VuetifyTheme",
                "OrchardCore.ContentTypes",
                "StatCan.OrchardCore.DisplayHelpers",
                "StatCan.OrchardCore.Menu",
                Constants.Features.Vuetify,
                Constants.Features.Alert,
                Constants.Features .Card,
                Constants.Features .ExpansionPanel,
                Constants.Features.Grid,
                Constants.Features.Image,
                Constants.Features.List,
                Constants.Features.Schedule,
                Constants.Features.Tabs,
                Constants.Features.Timeline,
            });
            await _shellFeaturesManager.EnableFeaturesAsync(vuetifyModule, true);
            await _siteThemeService.SetSiteThemeAsync("VuetifyTheme");
        }
    }
}
