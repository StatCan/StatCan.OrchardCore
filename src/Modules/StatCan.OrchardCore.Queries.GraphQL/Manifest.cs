using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "GraphQL Queries",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.Queries.GraphQL",
    Name = "GraphQL Queries",
    Description = "Introduces a way to create custom Queries using the GraphQL Language",
    Category = "Scheduling",
    Dependencies = new []
    {
        "OrchardCore.Queries",
        "OrchardCore.Apis.GraphQL"
    }
)]
