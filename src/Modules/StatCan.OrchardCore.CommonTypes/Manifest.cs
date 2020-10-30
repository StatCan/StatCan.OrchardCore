using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.CommonTypes.FeatureIds;

[assembly: Module(
    Name = "StatCan CommonTypes",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Pages,
    Name = "StatCan.CommonTypes - Pages",
    Category = "Content",
    Description = "Adds generic page content types such as Page, Liquid Page, Html Page and MarkdownPage",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
    }
)]

[assembly: Feature(
    Id = Widgets,
    Name = "StatCan.CommonTypes - Widgets",
    Category = "Content",
    Description = "Adds generic widget content types such as Liquid, Html, Markdown and Container widgets",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
    }
)]