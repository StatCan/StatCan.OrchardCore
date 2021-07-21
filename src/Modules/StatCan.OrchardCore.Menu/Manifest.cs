using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "StatCan.OrchardCore.Menu",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Menu overrides",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Menu" }
)]
