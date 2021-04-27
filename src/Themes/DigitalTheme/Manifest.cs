using OrchardCore.DisplayManagement.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Theme(
    Name = "DigitalTheme",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Theme of Innovation Team website",
    Dependencies = new[] {
        "OrchardCore.ContentTypes"
    }
)]
