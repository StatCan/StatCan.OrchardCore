using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "StatCan.OrchardCore.Users",
    Author = "Digital Innovation Team",
    Website = "https://innovation.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Statcan users module fixes and modifications",
    Category = "Security",
    Dependencies = new[] { "OrchardCore.Users" }
)]
