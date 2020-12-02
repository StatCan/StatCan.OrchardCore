using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "StatCan.OrchardCore.DisplayHelpers",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Additional liquid filters",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]
