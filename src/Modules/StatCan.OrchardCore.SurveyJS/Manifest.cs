using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.SurveyJS.FeatureIds;

[assembly: Module(
    Name = "StatCan SurveyJS",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = SurveyJS,
    Name = "StatCan.SurveyJS - Widgets",
    Category = "Content",
    Description = "Adds a widget used to create SurveyJS Assessments",
    Dependencies = new[]
    {
        "OrchardCore.Widgets",
        
    }
)]
