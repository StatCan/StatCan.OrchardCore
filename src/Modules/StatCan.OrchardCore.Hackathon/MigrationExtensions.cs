
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;

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
    }
}