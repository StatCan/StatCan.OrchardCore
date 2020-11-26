using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using static StatCan.OrchardCore.Hackathon.FeatureIds;

[assembly: Module(
    Name = "Hackathons",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version
)]

[assembly: Feature(
    Id = Hackathon,
    Name = "Hackathon",
    Description = "Manages types and templates for hackathon support",
    Dependencies = new[] {
        "OrchardCore.Contents",
        "OrchardCore.Workflows",
        "OrchardCore.Workflows.Http",
        "OrchardCore.Autoroute",
        "OrchardCore.Queries.Sql",
        "StatCan.OrchardCore.VueForms.Localized"
    },
    Category = "Hackathon"
)]

[assembly: Feature(
    Id = Team,
    Name = "Hackathon Teams",
    Description = "Provides Teams management for the Hackathon feature",
    Dependencies = new[] { Hackathon },
    Category = "Hackathon"
)]

[assembly: Feature(
    Id =  Judging,
    Name = "Hackathon Judging",
    Description = "Provides Juding management for the Hackathon feature",
    Dependencies = new[] { Team },
    Category = "Hackathon"
)]
