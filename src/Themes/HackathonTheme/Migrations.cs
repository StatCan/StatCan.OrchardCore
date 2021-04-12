
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Themes.Services;
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

        public int Create()
        {
            MigrateToVuetifyTheme();
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
        public int UpdateFrom7()
        {
            MigrateToVuetifyTheme();
            return 8;
        }

        private async void MigrateToVuetifyTheme()
        {
            var vuetify = _extensionManager.GetFeatures(new []{"VuetifyTheme"});
            await _shellFeaturesManager.EnableFeaturesAsync(vuetify);

            await _siteThemeService.SetSiteThemeAsync("VuetifyTheme");
        }
    }
}
