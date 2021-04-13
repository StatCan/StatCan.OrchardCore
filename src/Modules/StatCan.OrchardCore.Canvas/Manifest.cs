using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Canvas.FeatureIds;

[assembly: Module(
    Name = "StatCan Canvas",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Canvas,
    Name = "StatCan.Canvas - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create Business and Client Canvas.",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
