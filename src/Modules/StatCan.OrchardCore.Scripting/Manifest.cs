using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "StatCan.OrchardCore.Scripting",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Scripting methods",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Scripting" }
)]
