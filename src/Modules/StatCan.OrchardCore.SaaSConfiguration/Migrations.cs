using OrchardCore.Data.Migration;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class Migrations : DataMigration
    {
        private readonly ISaaSConfigurationService _configurationService;

        public Migrations(ISaaSConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<int> CreateAsync()
        {
            await _configurationService.GenerateDefaultSaaSConfigurationSettingsAsync();
            await _configurationService.SetOpenIdServerDefaultSettingsAsync();
            await _configurationService.CreateApplicationAsync();
            await _configurationService.CreateScopesAsync();
            await _configurationService.UpdateTenantsClientSettingsAsync();
            return 1;
        }
    }
}
