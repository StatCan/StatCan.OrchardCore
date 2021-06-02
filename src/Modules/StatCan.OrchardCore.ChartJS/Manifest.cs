using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.ChartJS.FeatureIds;

[assembly: Module(
    Name = "StatCan ChartJS",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = ChartJS,
    Name = "StatCan.ChartJS - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create Line, Pie and Bar charts.",
    Dependencies = new[]
    {
        "OrchardCore.Widgets"
    }
)]