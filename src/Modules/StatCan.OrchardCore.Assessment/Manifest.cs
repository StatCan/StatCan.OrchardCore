using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Assessment.FeatureIds;

[assembly: Module(
    Name = "StatCan Assessment",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Assessment,
    Name = "StatCan.Assessment - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create assessments",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
