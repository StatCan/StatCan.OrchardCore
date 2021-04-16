using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OpenIddict.Abstractions;
using OrchardCore.Entities;
using OrchardCore.Environment.Extensions;
using OrchardCore.Environment.Shell;
using OrchardCore.OpenId.Abstractions.Descriptors;
using OrchardCore.OpenId.Abstractions.Managers;
using OrchardCore.OpenId.Configuration;
using OrchardCore.OpenId.Services;
using OrchardCore.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;
using static OrchardCore.OpenId.Settings.OpenIdServerSettings;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class SaaSConfigurationService : ISaaSConfigurationService
    {
        private readonly IOpenIdServerService _serverService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOpenIdApplicationManager _applicationManager;
        private readonly IOpenIdScopeManager _scopeManager;
        private readonly IIdGenerator _idGenerator;
        private readonly ISiteService _siteService;
        private readonly ShellSettings _shellSettings;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IDataProtector _protector;
        private readonly IShellHost _shellHost;


        public SaaSConfigurationService(IOpenIdServerService serverService,
            IHttpContextAccessor httpContextAccessor,
            IOpenIdApplicationManager applicationManager,
            IOpenIdScopeManager scopeManager,
            IDataProtectionProvider dataProtectionProvider,
            IIdGenerator idGenerator,
            IShellHost shellHost,
            ISiteService siteService,
            ShellSettings shellSettings
            )
        {
            _serverService = serverService;
            _httpContextAccessor = httpContextAccessor;
            _applicationManager = applicationManager;
            _scopeManager = scopeManager;
            _idGenerator = idGenerator;
            _siteService = siteService;
            _shellSettings = shellSettings;
            _dataProtectionProvider = dataProtectionProvider;
            _protector = _dataProtectionProvider.CreateProtector(Constants.Features.SaaSConfiguration);
            _shellHost = shellHost;
        }

        public async Task<SaaSConfigurationSettings> GetSettingsAsync()
        {
            var container = await _siteService.GetSiteSettingsAsync();
            return container.As<SaaSConfigurationSettings>();
        }

        public async Task<SaaSConfigurationSettings> LoadSettingsAsync()
        {
            var container = await _siteService.LoadSiteSettingsAsync();
            return container.As<SaaSConfigurationSettings>();
        }

        public async Task UpdateSettingsAsync(SaaSConfigurationSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var container = await _siteService.LoadSiteSettingsAsync();
            container.Properties[nameof(SaaSConfigurationSettings)] = JObject.FromObject(settings);
            await _siteService.UpdateSiteSettingsAsync(container);
        }

        public string GetUnprotectedClientSecret(string clientSecret)
        {
            return _protector.Unprotect(clientSecret);
        }
        public string GetProtectedClientSecret(string clientSecret)
        {
            return _protector.Protect(clientSecret);
        }

        public async Task GenerateDefaultSaaSConfigurationSettingsAsync()
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var settings = await LoadSettingsAsync();

            settings.ClientId = _idGenerator.GenerateUniqueId();
            settings.ClientSecret = GetProtectedClientSecret(_idGenerator.GenerateUniqueId());
            settings.Authority = new Uri(request.Scheme + "://" + request.Host + request.PathBase);

            await UpdateSettingsAsync(settings);
        }

        public async Task SetOpenIdServerDefaultSettingsAsync()
        {
            var settings = await _serverService.LoadSettingsAsync();
            settings.AccessTokenFormat = TokenFormat.DataProtection;
            settings.Authority = null;

            settings.AuthorizationEndpointPath = new PathString("/connect/authorize");
            settings.LogoutEndpointPath = new PathString("/connect/logout");
            settings.TokenEndpointPath = new PathString("/connect/token");
            settings.UserinfoEndpointPath = new PathString("/connect/userinfo");
            settings.AllowAuthorizationCodeFlow = true;
            settings.AllowClientCredentialsFlow = false;
            settings.AllowHybridFlow = false;
            settings.AllowImplicitFlow = false;
            settings.AllowPasswordFlow = false;
            settings.AllowRefreshTokenFlow = true;

            await _serverService.UpdateSettingsAsync(settings);
        }

        public async Task<object> GetApplicationAsync(string currentClientId)
        {
            return await _applicationManager.FindByClientIdAsync(currentClientId) ?? await CreateApplicationAsync();
        }

        public async Task UpdateApplicationAsync(string currentClientId, SaaSConfigurationSettings settings)
        {
            var application = await _applicationManager.FindByClientIdAsync(currentClientId);
            if (application != null)
            {
                var descriptor = new OpenIdApplicationDescriptor();
                await _applicationManager.PopulateAsync(descriptor, application);
                descriptor.ClientId = settings.ClientId;
                descriptor.ClientSecret = GetUnprotectedClientSecret(settings.ClientSecret);
                await _applicationManager.UpdateAsync(application, descriptor);
            }
            else
            {
                await CreateApplicationAsync(settings);
            }
        }

        public async ValueTask<object> CreateApplicationAsync()
        {
            return await CreateApplicationAsync(await GetSettingsAsync());
        }
        public async ValueTask<object> CreateApplicationAsync(SaaSConfigurationSettings settings)
        {
            var descriptor = new OpenIdApplicationDescriptor
            {
                ClientId = settings.ClientId,
                ClientSecret = GetUnprotectedClientSecret(settings.ClientSecret),
                ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                DisplayName = "SaaSConfiguration Application",
                Type = OpenIddictConstants.ClientTypes.Confidential
            };

            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);

            //add the allowed scopes for the application
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Profile);
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + OpenIddictConstants.Scopes.Email);
            return await _applicationManager.CreateAsync(descriptor);
        }

        //Comma seperated list of uri
        public async Task UpdateRedirectUris(string uris)
        {
            if(string.IsNullOrEmpty(uris))
            {
                return;
            }

            var settings = await GetSettingsAsync();
            var application = await GetApplicationAsync(settings.ClientId);

            var descriptor = new OpenIdApplicationDescriptor();
            await _applicationManager.PopulateAsync(descriptor, application);
            descriptor.RedirectUris.UnionWith(
                from uri in uris?.Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>()
                select new Uri(uri, UriKind.Absolute));
            await _applicationManager.UpdateAsync(application, descriptor);
        }

        public async Task CreateScopesAsync()
        {
            await AddScope(OpenIddictConstants.Scopes.Profile, "Profile, required for all tenants");
            await AddScope(OpenIddictConstants.Scopes.Email, "Email");
        }

        private ValueTask<object> AddScope(string name, string description)
        {
            var scope = new OpenIdScopeDescriptor
            {
                Name = name,
                DisplayName = name,
                Description = description,
            };

            return _scopeManager.CreateAsync(scope);
        }

        public async Task UpdateTenantsClientSettingsAsync()
        {
            await UpdateTenantsClientSettingsAsync(await GetSettingsAsync());
        }
        public async Task UpdateTenantsClientSettingsAsync(SaaSConfigurationSettings settings)
        {
            var unprotectedClientSecret = GetUnprotectedClientSecret(settings.ClientSecret);

            var shellSettings = _shellHost.GetAllSettings();
            var tenants = shellSettings.Where(t => !string.Equals(t.Name, _shellSettings.Name));
            var uris = String.Empty;
            foreach (var tenant in tenants)
            {
                var shellScope = await _shellHost.GetScopeAsync(tenant.Name);
                await shellScope.UsingAsync(async scope =>
                {
                    var extensionManager = scope.ServiceProvider.GetRequiredService<IExtensionManager>();
                    var dataProtectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();

                    var feature = extensionManager.GetFeatures().FirstOrDefault(f => f.Id == Constants.Features.SaaSConfigurationClient);
                    // tenant does not have our ConfigurationClient enabled, thus not configured by this module.
                    if (feature == null)
                    {
                        return;
                    }
                    // skip if the OpenIdClient is not enabled on the child tenant
                    var clientService = scope.ServiceProvider.GetService<IOpenIdClientService>();
                    if(clientService == null)
                    {
                        return;
                    }
                    var httpContextAccessor = scope.ServiceProvider.GetService<IHttpContextAccessor>();
                    var request = httpContextAccessor.HttpContext.Request;

                    var clientSettings = await clientService.LoadSettingsAsync();
                    clientSettings.ClientId = settings.ClientId;

                    var clientProtector = dataProtectionProvider.CreateProtector(nameof(OpenIdClientConfiguration));
                    clientSettings.ClientSecret = clientProtector.Protect(unprotectedClientSecret);

                    clientSettings.Authority = settings.Authority;

                    await clientService.UpdateSettingsAsync(clientSettings);

                    //todo: verify that the CallbackPath is not null here
                    var currentUri = new Uri(request.Scheme + "://" + request.Host + request.PathBase + clientSettings.CallbackPath);
                    uris += currentUri.AbsoluteUri + ",";

                    // Reload tenant after modifying it
                    await _shellHost.ReleaseShellContextAsync(tenant);
                });
            }

            await UpdateRedirectUris(uris);

            // Release the default tenant to update the OpenID Server.
            await _shellHost.ReleaseShellContextAsync(_shellSettings);
        }
    }
}
