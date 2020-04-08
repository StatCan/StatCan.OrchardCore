using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StatCan.OrchardCore.Workflows",
    Author = "Digital Innovation Team",
    Website = "https://innovation.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Utilities module ",
    Category = "Innovation",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]
