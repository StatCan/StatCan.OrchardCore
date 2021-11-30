using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using static StatCan.OrchardCore.Candev.FeatureIds;

[assembly: Module(
    Name = "Candev",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Contains types and utilities for Candev",
    Category = "Applications"
)]

[assembly: Feature(
    Id = Candev,
    Name = "Candev",
    Description = "Manages types and templates for hackathon support",
    Dependencies = new[] {
        "OrchardCore.Contents",
        "OrchardCore.Workflows",
        "OrchardCore.Workflows.Http",
        "OrchardCore.Autoroute",
        "OrchardCore.Queries.Sql",
        "OrchardCore.Users.CustomUserSettings",
        "StatCan.OrchardCore.VueForms.Localized",
        "StatCan.OrchardCore.EmailTemplates"
    },
    Category = "Applications"
)]
