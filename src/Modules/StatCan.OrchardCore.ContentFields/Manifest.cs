using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.ContentFields;

[assembly: Module(
    Name = "statCan ContentFields",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Additional content fields",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.Liquid" }
)]

[assembly: Feature(
    Id = Constants.Features.PredefinedGroup,
    Name = "PredefinedGroup Field",
    Description = "TextField 'Predefined List' editor that allows using svg's or html as names.",
    Category = "Content"
)]

