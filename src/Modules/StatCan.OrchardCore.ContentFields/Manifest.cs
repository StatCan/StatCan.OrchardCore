using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.ContentFields;

[assembly: Module(
    Name = "StatCan ContentFields",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version,
    Description = "Additional content fields",
    Category = "Content"
)]

[assembly: Feature(
    Id = Constants.Features.ContentFields,
    Name = "StatCan additional ContentFields",
    Description = "Adds editors to existing content fields",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentFields" }
)]

[assembly: Feature(
    Id = Constants.Features.PredefinedGroup,
    Name = "StatCan PredefinedGroup Field",
    Description = "TextField 'Predefined List' editor that allows using svg's or html as names.",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentFields" }
)]

[assembly: Feature(
    Id = Constants.Features.MultiValueTextField,
    Name = "StatCan MultiValue TextField",
    Description = "Field for choosing multiple values from collection of configured options.",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentFields" }
)]
