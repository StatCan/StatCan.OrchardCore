using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Hackathon;

[assembly: Module(
    Name = "Hackathons",
    Author = "Digital Innovation Team",
    Website = "https://innovation.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = FeatureIds.Hackathon,
    Name = "Hackathon",
    Description = "Manages types and templates for hackathon support",
    Dependencies = new[] {
        "OrchardCore.Contents",
        "OrchardCore.Workflows",
        "OrchardCore.Workflows.Http",
        "OrchardCore.Autoroute",
        "OrchardCore.Queries.Sql",
        "OrchardCore.User.CustomUserSettings",
        "StatCan.OrchardCore.VueForms.Localized"
    },
    Category = "Hackathon"
)]

[assembly: Feature(
    Id = FeatureIds.Team,
    Name = "Hackathon Teams",
    Description = "Provides Teams management for the Hackathon feature",
    Dependencies = new[] { FeatureIds.Hackathon },
    Category = "Hackathon"
)]

[assembly: Feature(
    Id =  FeatureIds.Judging,
    Name = "Hackathon Judging",
    Description = "Provides Juding management for the Hackathon feature",
    Dependencies = new[] { FeatureIds.Team },
    Category = "Hackathon"
)]
