using Microsoft.AspNetCore.DataProtection;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Scope;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public interface ITenantHelperService
    {
        IEnumerable<ShellSettings> GetTenantsExceptCurrent();
        IEnumerable<ShellSettings> GetTenantsExceptDefault();
        Task ExecuteForTenants(IEnumerable<ShellSettings> tenants, Func<ShellSettings, ShellScope, Task> execute);
        Task DisableFeatureAsync(string featureId, bool force = false);
        Task EnableFeatureAsync(string featureId, bool force = false);
    }

    public class TenantHelperService : ITenantHelperService
    {
        private readonly IExtensionManager _extensionManager;
        private readonly IShellHost _shellHost;
        private readonly IShellFeaturesManager _shellFeaturesManager;
        private readonly ShellSettings _shellSettings;

        public TenantHelperService(IShellHost shellHost,
            ShellSettings shellSettings,
            IExtensionManager extensionManager,
            IShellFeaturesManager shellFeaturesManager)
        {
            _shellHost = shellHost;
            _shellSettings = shellSettings;
            _extensionManager = extensionManager;
            _shellFeaturesManager = shellFeaturesManager;
        }

        public IEnumerable<ShellSettings> GetTenantsExceptDefault()
        {
            var shellSettings = _shellHost.GetAllSettings();
            return shellSettings.Where(t => !string.Equals(t.Name, ShellHelper.DefaultShellName));
        }

        public IEnumerable<ShellSettings> GetTenantsExceptCurrent()
        {
            var shellSettings = _shellHost.GetAllSettings();
            return shellSettings.Where(t => !string.Equals(t.Name, _shellSettings.Name));
        }

        public async Task ExecuteForTenants(IEnumerable<ShellSettings> tenants, Func<ShellSettings, ShellScope, Task> execute)
        {
            foreach (var tenant in tenants)
            {
                var shellScope = await _shellHost.GetScopeAsync(tenant.Name);
                await shellScope.UsingAsync(async scope => await execute(tenant, scope));
            }
        }

        public async Task DisableFeatureAsync(string featureId, bool force = false)
        {
           var feature = _extensionManager.GetFeatures(new []{featureId});
           await _shellFeaturesManager.DisableFeaturesAsync(feature, force);
        }

        public async Task EnableFeatureAsync(string featureId, bool force = false)
        {
           var feature = _extensionManager.GetFeatures(new []{featureId});
           await _shellFeaturesManager.EnableFeaturesAsync(feature, force);
        }
    }
}
