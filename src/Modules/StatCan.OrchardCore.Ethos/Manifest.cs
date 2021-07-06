using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Persona.FeatureIds;

[assembly: Module(
    Name = "StatCan Persona",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Persona,
    Name = "StatCan.Persona - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create a Persona",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
