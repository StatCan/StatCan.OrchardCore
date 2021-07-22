using OrchardCore.DisplayManagement.Manifest;
using static StatCan.OrchardCore.Manifest.StatCanManifestConstants;

[assembly: Theme(
    Name = "VuetifyTheme",
    Author = DigitalInnovationTeam,
    Website = DigitalInnovationWebsite,
    Version = "1.0.",
    Description = "Vuetify platform theme",
    Dependencies = new[] {
        "OrchardCore.ContentTypes",
        "StatCan.OrchardCore.DisplayHelpers",
        "StatCan.OrchardCore.Menu"
    }
)]
