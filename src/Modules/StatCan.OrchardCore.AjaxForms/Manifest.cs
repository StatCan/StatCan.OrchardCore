using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "AjaxForms",
    Author = "Jean-Philippe Tissot",
    Website = "https://orchardproject.net",
    Version = "1.0.0",
    Description = "The AjaxForms module provides form management features.",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "OrchardCore.Title",
        "OrchardCore.Fields"
    },
    Category = "Form"
)]
