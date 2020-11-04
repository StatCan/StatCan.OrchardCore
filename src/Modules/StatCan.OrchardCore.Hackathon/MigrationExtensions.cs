
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
        public static ContentPartDefinitionBuilder WithCaseField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Case", f => f
                .OfType(nameof(LocalizationSetContentPickerField))
                .WithDisplayName("Case")
                .WithPosition(position)
                .WithSettings(new LocalizationSetContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Case" } })
            );
        }

        public static ContentPartDefinitionBuilder WithTeamField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Team", f => f
                    .OfType(nameof(ContentPickerField))
                    .WithDisplayName("Team")
                    .WithPosition(position)
                    .WithSettings(new ContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Team" } })
                );
        }

        public static ContentTypeDefinitionBuilder WithTitlePart(this ContentTypeDefinitionBuilder t, string position)
        {
            return t.WithPart(nameof(TitlePart), p => p
                .WithPosition(position)
            );
        }

        public static ContentTypeDefinitionBuilder WithTitlePart(this ContentTypeDefinitionBuilder t, string position, TitlePartOptions options = TitlePartOptions.Editable, string pattern = "")
        {
            return t.WithPart(nameof(TitlePart), p => p
                .WithPosition(position)
                .WithSettings(new TitlePartSettings(){Options = options, Pattern = pattern})
            );
        }
    }
}