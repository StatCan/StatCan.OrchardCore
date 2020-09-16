using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "VueForms",
    Author = "Jean-Philippe Tissot - Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0",
    Description = "The VueForm module provides a form ContentType that simplifies using VueJs forms in the frontend.",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "OrchardCore.Title",
        "OrchardCore.Flows",
        "StatCan.OrchardCore.ContentFields"
    },
    Category = "Form"
)]
