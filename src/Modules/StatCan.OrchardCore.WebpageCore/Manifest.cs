using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.WebpageCore;

[assembly: Module(
    Name = "StatCan WebpageCore",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Constants.Features.PageLayout,
    Name = "StatCan Page Layout",
    Description = "A collection of common website layout components",
    Category = "StatCan.OrchardCore.WebpageCore",
    Dependencies = new[]
    {
        "OrchardCore.Liquid",
        "OrchardCore.Widgets",
        "StatCan.OrchardCore.ContentFields.PredefinedGroup",
    }
)]

[assembly: Feature(
    Id = Constants.Features.ContentLayout,
    Name = "StatCan Content Layout",
    Description = "A collection of common content layout components",
    Category = "StatCan.OrchardCore.WebpageCore",
    Dependencies = new[]
    {
        "OrchardCore.Liquid",
        "OrchardCore.Widgets",
    }
)]

[assembly: Feature(
    Id = Constants.Features.MenuItemParts,
    Name = "StatCan MenuParts",
    Description = "A collection of common menu item parts",
    Category = "StatCan.OrchardCore.WebpageCore",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        "OrchardCore.Liquid",
    }
)]