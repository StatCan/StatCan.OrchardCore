using OrchardCore.Modules.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;
using StatCan.OrchardCore.Vuetify;

[assembly: Module(
    Name = "Vuetify Module",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = Version
)]

[assembly: Feature(
    Id = Constants.Features.Vuetify,
    Name = "Vuetify",
    Description = "The Vuetify module provides widgets and extras for the Vuetify theme.",
    Category = "Vuetify",
    Dependencies = new[]
    {
        "OrchardCore.Contents",
        "StatCan.OrchardCore.ContentFields",
        "OrchardCore.Flows",
        "OrchardCore.Liquid"
    }
)]

[assembly: Feature(
    Id = Constants.Features.Alert,
    Name = "VAlert",
    Description = "Adds the VAlert widget.",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Card,
    Name = "VCard",
    Description = "Adds the VCard widget",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.ExpansionPanel,
    Name = "VExpansionPanel",
    Description = "Adds the VExpansionPanels and VExpansionPanel widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Grid,
    Name = "VGrid",
    Description = "Adds the VContainer, VRow, VCol and VDivider widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Image,
    Name = "VImage",
    Description = "Adds the VImg widget",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.List,
    Name = "VList",
    Description = "Adds the VList, VListItem, VSubheader widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Schedule,
    Name = "VSchedule",
    Description = "Adds the ScheduleList and ScheduleEvent widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Tabs,
    Name = "VTabs",
    Description = "Adds the Tabs and Tab widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
[assembly: Feature(
    Id = Constants.Features.Timeline,
    Name = "VTimeline",
    Description = "Adds the VTimeline and VTimelineItem widgets",
    Category = "Vuetify",
    Dependencies = new[] { Constants.Features.Vuetify }
)]
