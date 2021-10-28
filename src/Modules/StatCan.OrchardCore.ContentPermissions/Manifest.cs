using OrchardCore.Modules.Manifest;
using Etch.OrchardCore.ContentPermissions;

[assembly: Module(
    Author = "Etch UK Ltd.",
    Category = "Security",
    Description = "Configuring access at a per content item level.",
    Name = "Content Permissions",
    Version = "0.3.1",
    Website = "https://etchuk.com"
)]

[assembly: Feature(
    Id = Constants.Features.ContentPermissions,
    Name = "Content Permissions",
    Category = "Security",
    Description = "Creates a ContentPermissionsPart that allows configuring access at a per content item level."
)]

[assembly: Feature(
    Id = Constants.Features.Indexing,
    Name = "Content Permissions Indexing",
    Category = "Security",
    Description = "Creates an index table for ContentPermissionsPart",
    Dependencies = new[]
    {
        "Etch.OrchardCore.ContentPermissionsPart"
    }
)]
