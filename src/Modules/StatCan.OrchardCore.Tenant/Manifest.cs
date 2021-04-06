using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Tenant.FeatureIds;

[assembly: Module(
    Name = "StatCan Tenant",
    Author = "Digital Innovation Team",
    Website = "https://digital.statcan.gc.ca",
    Version = "1.0.0"
)]

[assembly: Feature(
    Id = Tenant,
    Name = "StatCan.Tenant - Widgets",
    Category = "Content",
    Description = "Adds a widget used to display a List of Tenant and its information",
    Dependencies = new[]
    {
        "OrchardCore.Widgets"
    }
)]