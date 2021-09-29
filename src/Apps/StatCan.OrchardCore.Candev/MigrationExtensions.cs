
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;

namespace StatCan.OrchardCore.Candev
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
                    .OfType(nameof(UserPickerField))
                    .WithDisplayName("Team Captain")
                    .WithPosition(position)
                    .WithSettings(new UserPickerFieldSettings() { DisplayedRoles = new string[] { "Hacker" } })
                );
        }
    }
}
