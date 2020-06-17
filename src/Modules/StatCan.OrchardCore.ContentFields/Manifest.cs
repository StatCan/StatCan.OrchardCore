using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StatCan.OrchardCore.ContentFields",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Additional content fields",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]
