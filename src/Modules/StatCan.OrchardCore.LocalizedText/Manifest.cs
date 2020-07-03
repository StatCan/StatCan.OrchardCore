using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.LocalizedText;

[assembly: Module(
    Name = "LocalizedText Part",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Localization part that allows storing localized strings in a single content item",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentLocalization" }
)]

[assembly: Feature(
    Id = Constants.Features.LocalizedText,
    Name = "LocalizedText Part",
    Description = "Part for managing localized strings inside a single ContentItem",
    Category = "Content"
)]
