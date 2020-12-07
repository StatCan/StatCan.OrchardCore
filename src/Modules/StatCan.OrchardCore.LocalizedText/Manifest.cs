using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.LocalizedText;

[assembly: Module(
    Name = "LocalizedText Part",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Localization part that allows storing localized strings in a single content item",
    Category = "Content"
)]

[assembly: Feature(
    Id = Constants.Features.LocalizedText,
    Name = "LocalizedText Part",
    Description = "Part for managing localized strings inside a single ContentItem",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.Localization" }
)]
