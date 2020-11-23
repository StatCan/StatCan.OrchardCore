
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Title.Models;

namespace StatCan.OrchardCore.Hackathon
{
    /// <summary>
    /// Content Type / Part builder extensions to simplify migrations
    /// </summary>
    public static class MigrationExtensions
    {
        public static ContentPartDefinitionBuilder WithTeamField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition(position)
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Team" } })
                );
        }

        public static ContentPartDefinitionBuilder WithChallengeField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Challenge", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Challenge")
                    .WithPosition(position)
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Challenge" } })
                );
        }

        public static ContentPartDefinitionBuilder WithTeamCaptainField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("TeamCaptain", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team Captain")
                    .WithPosition(position)
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Hacker" } })
                );
        }
    }
}
