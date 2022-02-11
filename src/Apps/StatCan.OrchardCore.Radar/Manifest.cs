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
    Dependencies = new[]
    {
        "OrchardCore.Admin",
        "OrchardCore.Alias",
        "OrchardCore.Autoroute",
        "OrchardCore.Contents",
        "OrhcardCore.ContentManagement",
        "OrchardCore.ContentLocalization",
        "OrchardCore.ContentLocalization.ContentCulturePicker",
        "OrchardCore.ContentFields",
        "OrchardCore.Features",
        "OrchardCore.Flows",
        "OrchardCore.HomeRoute",
        "OrchardCore.Indexing",
        "OrchardCore.Liquid",
        "OrchardCore.Lucene",
        "OrchardCore.Layers",
        "OrchardCore.Localization",
        "OrchardCore.Media",
        "OrchardCore.Menu",
        "OrchardCore.Queries",
        "OrchardCore.Queries.Sql",
        "OrchardCore.Resources",
        "OrchardCore.Roles",
        "OrchardCore.Settings",
        "OrchardCore.Shortcodes",
        "OrchardCore.Taxonomies",
        "OrchardCore.Themes",
        "OrchardCore.Title",
        "OrchardCore.Users",
        "OrchardCore.Users.CustomUserSettings",
        "OrchardCore.Users.Registration",
        "OrchardCore.Widgets",
        "StatCan.OrchardCore.ContentPermissions.Indexing",
        "StatCan.OrchardCore.DisplayHelpers",
        "StatCan.OrchardCore.Menu",
        "StatCan.OrchardCore.VueForms.Localized"
    }
)]
