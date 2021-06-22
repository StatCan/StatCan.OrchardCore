using System.Threading.Tasks;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions.Features;
using OrchardCore.Environment.Shell;
using OrchardCore.Settings;
using OrchardCore.Environment.Shell.Configuration;
using Microsoft.Extensions.Configuration;
using OrchardCore.Modules;
using OrchardCore.Https.Settings;
using OrchardCore.Environment.Extensions;
using System.Linq;
using OrchardCore.DisplayManagement.Notify;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc.Localization;

namespace StatCan.OrchardCore.Configuration
{
    /// <summary>
    /// Updates the Https settings based on a configuration
    /// </summary>
    public class PersistentFeatures : ModularTenantEvents, IFeatureEventHandler
    {
        private readonly ShellSettings _shellSettings;
        private readonly IShellHost _shellHost;
        private readonly IShellConfiguration _shellConfiguration;
        private readonly IExtensionManager _extensionManager;
        private readonly IShellFeaturesManager _shellFeaturesManager;

        public PersistentFeatures(
            IShellConfiguration shellConfiguration,
            IExtensionManager extensionManager,
            IShellFeaturesManager shellFeaturesManager,
            ShellSettings shellSettings,
            IShellHost shellHost)
        {
            _shellConfiguration = shellConfiguration;
            _extensionManager = extensionManager;
            _shellFeaturesManager = shellFeaturesManager;
            _shellSettings = shellSettings;
            _shellHost = shellHost;
        }

        // for existing sites, update settings from configuration when tenant activates
        public async override Task ActivatedAsync()
        {
            var section = _shellConfiguration.GetSection("StatCan_PersistentFeatures");

            var configFeatures = section.Get<string[]>();

            if(configFeatures != null)
            {
                var enabledFeatures = await _shellFeaturesManager.GetEnabledFeaturesAsync();
                if(configFeatures.Any(x => !enabledFeatures.Any(y => y.Id == x)))
                {
                    var features = _extensionManager.GetFeatures().ToList();
                    var selectedFeatures = features.Where(f => configFeatures.Contains(f.Id)).ToList();
                    await _shellFeaturesManager.EnableFeaturesAsync(selectedFeatures, force: true);
                    await _shellHost.ReleaseShellContextAsync(_shellSettings);
                }
            }
        }

        Task IFeatureEventHandler.InstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.InstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.EnabledAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.DisablingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.DisabledAsync(IFeatureInfo feature) => CheckFeature(feature);

        Task IFeatureEventHandler.UninstallingAsync(IFeatureInfo feature) => Task.CompletedTask;

        Task IFeatureEventHandler.UninstalledAsync(IFeatureInfo feature) => Task.CompletedTask;

        private async Task CheckFeature(IFeatureInfo feature)
        {
            var section = _shellConfiguration.GetSection("StatCan_PersistentFeatures");

            var configFeatures = section.Get<string[]>();

            if(configFeatures?.Contains(feature.Id) == true)
            {
               await _shellFeaturesManager.EnableFeaturesAsync(new IFeatureInfo[]{feature}, force: true);
               await _shellHost.ReleaseShellContextAsync(_shellSettings);
            }
        }
    }
}
