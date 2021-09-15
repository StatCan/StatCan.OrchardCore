using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Radar;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "Digital Radar",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "The Digital Radar platform",
    Category = "Applications"
)]

[assembly: Feature(
    Id = Constants.Features.Radar,
    Name = "Radar",
    Description = "The Digital Radar Application",
    Category = "Applications",
    Dependencies = new []
    {
        // SaaS
        "OrchardCore.HomeRoute",
        "OrchardCore.Admin",
        "OrchardCore.Diagnostics",
        "OrchardCore.DynamicCache",
        "OrchardCore.Features",
        "OrchardCore.Navigation",
        "OrchardCore.Recipes",
        "OrchardCore.Resources",
        "OrchardCore.Roles",
        "OrchardCore.Settings",
        "OrchardCore.Themes",
        "OrchardCore.Users",

        // CMS
        "OrchardCore.Alias",
        "OrchardCore.AdminMenu",
        "OrchardCore.Autoroute",
        "OrchardCore.Html",
        "OrchardCore.ContentFields",
        "OrchardCore.ContentPreview",
        "OrchardCore.Contents",
        "OrchardCore.Contents.FileContentDefinition",
        "OrchardCore.ContentTypes",
        "OrchardCore.CustomSettings",
        "OrchardCore.Deployment",
        "OrchardCore.Deployment.Remote",
        "OrchardCore.Feeds",
        "OrchardCore.Flows",
        "OrchardCore.Indexing",
        "OrchardCore.Layers",
        "OrchardCore.Lucene",
        "OrchardCore.Lists",
        "OrchardCore.Markdown",
        "OrchardCore.Media",
        "OrchardCore.Menu",
        "OrchardCore.Placements",
        "OrchardCore.Queries",
        "OrchardCore.Shortcodes.Templates",
        "OrchardCore.Rules",
        "OrchardCore.Taxonomies",
        "OrchardCore.Title",
        "OrchardCore.Templates",
        "OrchardCore.Widgets",
    }
)]
