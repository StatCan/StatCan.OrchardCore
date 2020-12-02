using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.Matomo;

[assembly: Module(
    Name = MatomoConstants.Features.Matomo,
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Integrate Matomo analytics js",
    Category = "Matomo"
)]
