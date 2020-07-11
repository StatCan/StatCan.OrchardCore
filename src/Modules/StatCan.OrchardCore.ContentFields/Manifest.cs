using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.ContentFields;

[assembly: Module(
    Name = "StatCan ContentFields",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Additional content fields",
    Category = "Content"
)]

[assembly: Feature(
    Id = Constants.Features.ContentFields,
    Name = "Additional ContentFields",
    Description = "Adds editors to existing content fields",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentFields" }
)]

[assembly: Feature(
    Id = Constants.Features.PredefinedGroup,
    Name = "PredefinedGroup Field",
    Description = "TextField 'Predefined List' editor that allows using svg's or html as names.",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.ContentFields" }
)]

