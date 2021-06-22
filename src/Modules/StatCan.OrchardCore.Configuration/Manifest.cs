using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "StatCan Configuration",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Deployment configuration",
    Category = "Configuration"
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.Configuration",
    Name = "StatCan Configuration",
    Description = "Deployment configuration",
    Category = "Configuration",
    Dependencies = new[]
    {
       "OrchardCore.Features",
       "OrchardCore.Settings",
    }
)]
