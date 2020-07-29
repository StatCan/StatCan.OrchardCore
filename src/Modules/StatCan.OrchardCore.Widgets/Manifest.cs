using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Widgets;

[assembly: Module(
    Name = "StatCan Widgets",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Constants.Features.Widgets,
    Name = "StatCan Widgets",
    Description = "A collection of common website Widgets",
    Category = "Widget",
    Dependencies = new[]
    {
        "OrchardCore.Liquid",
    }
)]

[assembly: Feature(
    Id = Constants.Features.Page,
    Name = "Page",
    Description = "A container for Heroes and Sections widgets.",
    Category = "Widget",
    Dependencies = new[] {
        Constants.Features.Widgets
    }
)]

[assembly: Feature(
    Id = Constants.Features.Hero,
    Name = "Hero",
    Description = "A hero section",
    Category = "Widget",
    Dependencies = new[] {
        Constants.Features.Widgets
    }
)]

[assembly: Feature(
    Id = Constants.Features.Section,
    Name = "Section",
    Description = "Layout widgets to build a website",
    Category = "Widget",
    Dependencies = new[] {
        Constants.Features.Widgets
    }
)]

[assembly: Feature(
    Id = Constants.Features.FatFooter,
    Name = "Fat Footer",
    Description = "A footer widget that ties into the nav menu",
    Category = "Widget",
    Dependencies = new[] {
        Constants.Features.Widgets
    }
)]