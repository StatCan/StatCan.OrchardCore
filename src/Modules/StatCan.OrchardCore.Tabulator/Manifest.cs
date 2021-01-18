using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Tabulator.FeatureIds;

[assembly: Module(
    Name = "StatCan Table Creator",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Tabulator,
    Name = "StatCan.TableCreator - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create tables",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
