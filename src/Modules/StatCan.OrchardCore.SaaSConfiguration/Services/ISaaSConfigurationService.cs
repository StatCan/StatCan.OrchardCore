using System.Threading.Tasks;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public interface ISaaSConfigurationService
    {
        ValueTask<object> CreateApplicationAsync();
        ValueTask<object> CreateApplicationAsync(SaaSConfigurationSettings settings);
        Task CreateScopesAsync();
        Task GenerateDefaultSaaSConfigurationSettingsAsync();
        string GetProtectedClientSecret(string clientSecret);
        Task<SaaSConfigurationSettings> GetSettingsAsync();
        string GetUnprotectedClientSecret(string clientSecret);
        Task<SaaSConfigurationSettings> LoadSettingsAsync();
        Task SetOpenIdServerDefaultSettingsAsync();
        Task UpdateApplicationAsync(string currentClientId, SaaSConfigurationSettings settings);
        Task UpdateSettingsAsync(SaaSConfigurationSettings settings);
        Task UpdateTenantsClientSettingsAsync(SaaSConfigurationSettings settings);
        Task UpdateTenantsClientSettingsAsync();
        Task UpdateRedirectUris(string uris);
    }
}
