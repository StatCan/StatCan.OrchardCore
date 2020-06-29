using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.GCCollab;

[assembly: Module(
    Name = "GCCollab",
    Author = "StatCan Digital Innovation team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Category = "GCCollab"
)]

[assembly: Feature(
    Id = GCCollabConstants.Features.GCCollabAuthentication,
    Name = "GCCollab Authentication",
    Category = "GCCollab",
    Description = "Authenticates users with their GCCollab Account."
)]
