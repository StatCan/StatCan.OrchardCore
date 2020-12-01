using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.GCCollab;

[assembly: Module(
    Name = "GCCollab",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Category = "GCCollab"
)]

[assembly: Feature(
    Id = GCCollabConstants.Features.GCCollabAuthentication,
    Name = "GCCollab Authentication",
    Category = "GCCollab",
    Description = "Authenticates users with their GCCollab Account."
)]
