using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.Scheduling;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Module(
    Name = "StatCan Scheduling",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Contains types and utilities to build scheduling systems",
    Category = "Scheduling"
)]

[assembly: Feature(
    Id = Constants.Features.Scheduling,
    Name = "Scheduling Appointments",
    Description = "Provides types and utilities useful for scheduling appointments",
    Category = "Scheduling",
    Dependencies = new []
    {
        "OrchardCore.Contents",
        "OrchardCore.Taxonomies"
    }
)]
