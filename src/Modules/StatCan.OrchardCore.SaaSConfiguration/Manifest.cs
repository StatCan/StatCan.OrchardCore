using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.SaaSConfiguration;

[assembly: Module(
    Name = "SaaSConfiguration",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version
)]

[assembly: Feature(
    Id = Constants.Features.SaaSConfiguration,
    Name = "SaaS Configuration for Default tenant",
    Description = "SaaS configuration module. For default tenant.",
    Category = "Configuration",
    Dependencies = new [] { "OrchardCore.OpenId.Server", "OrchardCore.Tenants" },
    DefaultTenantOnly = true
)]

[assembly: Feature(
    Id = Constants.Features.SaaSConfigurationClient,
    Name = "SaaS Configuration for client tenants",
    Description = "SaaS tenant automatic configuration. For child tenants.",
    Category = "Configuration",
    Dependencies = new[] { "OrchardCore.OpenId.Client" }
)]
