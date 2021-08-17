using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using static StatCan.OrchardCore.CommonTypes.FeatureIds;

[assembly: Module(
    Name = "StatCan CommonTypes",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version
)]

[assembly: Feature(
    Id = Page,
    Name = "StatCan.CommonTypes - Page",
    Category = "Content",
    Description = "Adds generic Page content type",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "StatCan.OrchardCore.ContentPermissions",
        "OrchardCore.Flows",
        "OrchardCore.Liquid",
        "OrchardCore.Title"
    }
)]
[assembly: Feature(
    Id = SecurePage,
    Name = "StatCan.CommonTypes - Secure Page",
    Category = "Content",
    Description = "(DEPRECATED) Adds a page content types that has the ContentPermission part attached to it.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "StatCan.OrchardCore.ContentPermissions",
        "OrchardCore.Flows",
        "OrchardCore.Title",
        "OrchardCore.Liquid"
    }
)]

[assembly: Feature(
    Id = AdditionalPages,
    Name = "StatCan.CommonTypes - Additional Pages",
    Category = "Content",
    Description = "Adds generic page content types such as Liquid Page, Html Page and MarkdownPage",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Markdown",
        "OrchardCore.Title"
    }
)]

[assembly: Feature(
    Id = HtmlWidget,
    Name = "StatCan.CommonTypes - HtmlWidget",
    Category = "Content",
    Description = "",
    Dependencies = new[]
    {
        "OrchardCore.Html",
    }
)]
[assembly: Feature(
    Id = MarkdownWidget,
    Name = "StatCan.CommonTypes - MarkdownWidget",
    Category = "Content",
    Description = "",
    Dependencies = new[]
    {
        "OrchardCore.Markdown",
    }
)]
[assembly: Feature(
    Id = LiquidWidget,
    Name = "StatCan.CommonTypes - LiquidWidget",
    Category = "Content",
    Description = "",
    Dependencies = new[]
    {
        "OrchardCore.Liquid",
    }
)]

