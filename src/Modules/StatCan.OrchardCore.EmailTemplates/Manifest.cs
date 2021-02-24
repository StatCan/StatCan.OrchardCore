using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Manifest;

[assembly: Module(
    Name = "EmailTemplates",
    Author = StatCanManifestConstants.DigitalInnovationTeam,
    Website = StatCanManifestConstants.DigitalInnovationWebsite,
    Version = StatCanManifestConstants.Version
)]

[assembly: Feature(
    Id = "StatCan.OrchardCore.EmailTemplates",
    Name = "EmailTemplates",
    Description = "The EmailTemplates module provides a way to manage email templates",
    Dependencies = new[] { "OrchardCore.Liquid", "OrchardCore.Email" },
    Category = "Email"
)]
