using OrchardCore.DisplayManagement.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Theme(
    Name = "Digtal Admin Theme",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "A custom admin theme.",
    Tags = new [] { "Admin" },
    BaseTheme = "TheAdmin"
)]
