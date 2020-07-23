using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StatCan.OrchardCore.FIPHelper",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Federal Identity Program helper",
    Category = "StatCan",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]
