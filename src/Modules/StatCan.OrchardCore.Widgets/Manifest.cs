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
    Id = Constants.Features.MenuItemParts,
    Name = "StatCan MenuParts",
    Description = "A collection of common menu item parts",
    Category = "MenuItemPart",
    Dependencies = new[]
    {
        "OrchardCore.Liquid",
    }
)]