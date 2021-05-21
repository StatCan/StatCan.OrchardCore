using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Chartist.FeatureIds;

[assembly: Module(
    Name = "StatCan Chartist",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Chartist,
    Name = "StatCan.Chartist - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create Line, Pie and Bar charts.",
    Dependencies = new[]
    {
        "OrchardCore.Widgets"
    }
)]