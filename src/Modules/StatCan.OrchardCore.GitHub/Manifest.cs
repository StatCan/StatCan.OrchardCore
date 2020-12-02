using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.GitHub;

[assembly: Module(
    Name = GitHubConstants.Features.GitHub,
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Workflow tasks to interact with the github api.",
    Category = "GitHub",
    Dependencies = new string[] {"OrchardCore.Workflows"}
)]

