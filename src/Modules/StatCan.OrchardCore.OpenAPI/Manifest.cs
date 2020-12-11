using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.OpenAPI.FeatureIds;

[assembly: Module(
    Name = "StatCan OpenAPI",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = OpenAPI,
    Name = "StatCan.OpenAPI - Widgets",
    Category = "Content",
    Description = "Adds a widget that displays the Swagger UI",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        "OrchardCore.Title"
    }
)]
