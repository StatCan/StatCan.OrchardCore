using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.FIP.Constants;

[assembly: Module(
    Name = "FIP",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Fip,
    Name = "StatCan.FIP - Widgets",
    Category = "Content",
    Description = "Adds FIP widgets.",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        "OrchardCore.Title"
    }
)]
