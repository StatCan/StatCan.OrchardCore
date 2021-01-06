using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Ethos.FeatureIds;

[assembly: Module(
    Name = "StatCan Ethos",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Ethos,
    Name = "StatCan.Ethos - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create Ethos",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
