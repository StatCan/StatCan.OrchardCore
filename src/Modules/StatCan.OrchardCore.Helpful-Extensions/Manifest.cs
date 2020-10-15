using OrchardCore.Modules.Manifest;
using static Lombiq.HelpfulExtensions.FeatureIds;

[assembly: Module(
    Name = "Lombiq Helpful Extensions",
    Author = "Lombiq Technologies",
    Version = "1.0",
    Website = "https://github.com/Lombiq/Helpful-Extensions"
)]

[assembly: Feature(
    Id = CodeGeneration,
    Name = "Code Generation Helpful Extensions - Lombiq Helpful Extensions",
    Category = "Development",
    Description = "Generates migrations from content type definitions.",
    Dependencies = new[]
    {
        "OrchardCore.Resources",
    }
)]

[assembly: Feature(
    Id = Flows,
    Name = "Flows Helpful Extensions - Lombiq Helpful Extensions",
    Category = "Content",
    Description = "Adds additional styling capabilities to Flow Part.",
    Dependencies = new[]
    {
        "OrchardCore.Flows",
    }
)]

[assembly: Feature(
    Id = Widgets,
    Name = "Helpful Widgets - Lombiq Helpful Extensions",
    Category = "Content",
    Description = "Adds helpful widgets such as Container or Liquid widgets.",
    Dependencies = new[]
    {
        "OrchardCore.Html",
        "OrchardCore.Liquid",
        "OrchardCore.Title",
    }
)]

[assembly: Feature(
    Id = ContentTypes,
    Name = "Helpful Content Types - Lombiq Helpful Extensions",
    Category = "Content",
    Description = "Adds helpful content types such as Page.",
    Dependencies = new[]
    {
        "OrchardCore.Autoroute",
        "OrchardCore.Flows",
        "OrchardCore.Title",
    }
)]
