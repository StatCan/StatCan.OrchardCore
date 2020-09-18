using OrchardCore.Modules.Manifest;
using StatCan.OrchardCore.VueForms;

[assembly: Module(
    Name = "StatCan VueForms",
    Author = "Jean-Philippe Tissot - Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "The VueForm module provides a form ContentType that simplifies using VueJs forms in the frontend.",
    Category = "Form"
)]

[assembly: Feature(
    Id = Constants.Features.VueForms,
    Name = "VueForms",
    Description = "The VueForm module provides a form ContentType that simplifies using VueJs forms in the frontend.",
    Category = "Form",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "OrchardCore.Liquid",
        "StatCan.OrchardCore.Scripting"
    }
)]

[assembly: Feature(
    Id = Constants.Features.Localized,
    Name = "VueForms Localized",
    Description = "Welds the LocalizedText part to your VueForms ",
    Category = "Form",
    Dependencies = new[] { Constants.Features.VueForms, "StatCan.OrchardCore.LocalizedText" }
)]
