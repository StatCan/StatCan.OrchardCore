using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "Hackathons",
    Author = "Digital Innovation Team",
    Website = "https://innovation.statcan.gc.ca",
    Version = "1.0.0",
    Description = "Manages types and templates for hackathon support",
    Category = "Innovation"
)]

[assembly: Feature(
    Id = "StatCan.Orchardcore.Hackathon",
    Name = "Hackathon",
    Description = "Manages types and templates for hackathon support",
   // Dependencies = new[] {},
    Category = "Innovation"
)]

[assembly: Feature(
    Id = "StatCan.Orchardcore.Hackathon.Participants",
    Name = "Hackathon Judging",
    Description = "Provides Juding management for the Hackathon feature",
    Dependencies = new[] { "StatCan.Orchardcore.Hackathon" },
    Category = "Innovation"
)]

[assembly: Feature(
    Id = "StatCan.Orchardcore.Hackathon.Judging",
    Name = "Hackathon Judging",
    Description = "Provides Juding management for the Hackathon feature",
    Dependencies = new[] { "StatCan.Orchardcore.Hackathon" },
    Category = "Innovation"
)]