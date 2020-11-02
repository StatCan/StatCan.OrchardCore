using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Hackathons",
    Author = "Digital Innovation Team",
    Website = "https://innovation.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.Hackathon",
    Name = "Hackathon",
    Description = "Manages types and templates for hackathon support",
    Dependencies = new[] { "OrchardCore.Contents", "OrchardCore.Workflows", "OrchardCore.Workflows.Http", "OrchardCore.Autoroute", "OrchardCore.Queries.Sql" },
    Category = "Innovation"
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.Hackathon.Team",
    Name = "Hackathon Teams",
    Description = "Provides Teams management for the Hackathon feature",
    Dependencies = new[] { "StatCan.OrchardCore.Hackathon" },
    Category = "Innovation"
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.Hackathon.Judging",
    Name = "Hackathon Judging",
    Description = "Provides Juding management for the Hackathon feature",
    Dependencies = new[] { "StatCan.OrchardCore.Hackathon", "StatCan.OrchardCore.Hackathon.Team", "StatCan.OrchardCore.VueForms" },
    Category = "Innovation"
)]