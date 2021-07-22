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
        "OrchardCore.Flows",
        "OrchardCore.Liquid",
        "OrchardCore.Title"
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
    Id = Widgets,
    Name = "StatCan.CommonTypes - Widgets",
    Category = "Content",
    Description = "Adds generic widget content types such as Liquid, Html, Markdown and Container widgets",
    Dependencies = new[]
    {
        "OrchardCore.Html",
        "OrchardCore.Flows",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
        "OrchardCore.Markdown",
    }
)]

[assembly: Feature(
    Id = SecurePage,
    Name = "StatCan.CommonTypes - Secure Page",
    Category = "Content",
    Description = "Adds a page content types that has the ContentPermission part attached to it.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Title",
        "StatCan.OrchardCore.ContentPermissions"
    }
)]
