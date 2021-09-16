using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Radar;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "Digital Radar",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "The Digital Radar platform",
    Category = "Applications"
)]

[assembly: Feature(
    Id = Constants.Features.Radar,
    Name = "Radar",
    Description = "The Digital Radar Application",
    Category = "Applications",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Contents",
        "OrhcardCore.ContentManagement",
        "OrchardCore.ContentFields",
        "OrchardCore.Flows",
        "OrchardCore.Taxonomies",
        "OrchardCore.Title",
        "StatCan.OrchardCore.ContentPermissions",
    }
)]
