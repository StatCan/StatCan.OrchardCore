using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StatCan.OrchardCore.DisplayHelpers",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Additional liquid filters",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]
