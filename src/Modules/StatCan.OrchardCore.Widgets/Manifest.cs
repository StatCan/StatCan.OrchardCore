using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.ContentFields;

[assembly: Module(
    Name = "StatCan Widgets",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "A collection of common website Widgets",
    Category = "Widget",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]

[assembly: Feature(
    Id = Constants.Features.Page,
    Name = "Page",
    Description = "A container for Heroes and Sections widgets.",
    Category = "Widget"
)]

[assembly: Feature(
    Id = Constants.Features.Hero,
    Name = "Hero",
    Description = "A hero section",
    Category = "Widget"
)]

[assembly: Feature(
    Id = Constants.Features.Section,
    Name = "Section",
    Description = "Layout widgets to build a website",
    Category = "Widget"
)]

[assembly: Feature(
    Id = Constants.Features.FatFooter,
    Name = "Fat Footer",
    Description = "A footer widget that ties into the nav menu",
    Category = "Widget"
)]