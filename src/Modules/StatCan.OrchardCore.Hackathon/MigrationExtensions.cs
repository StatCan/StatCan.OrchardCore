
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
        public static ContentPartDefinitionBuilder WithHackathonField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Hackathon", f => f
                .OfType(nameof(LocalizationSetContentPickerField))
                .WithDisplayName("Hackathon")
                .WithPosition(position)
                .WithSettings(new LocalizationSetContentPickerFieldSettings() { Required = true, DisplayedContentTypes = new string[] { "Hackathon" } })
            );
        }

        public static ContentPartDefinitionBuilder WithCaseField(this ContentPartDefinitionBuilder p, string position)
        {
            return p.WithField("Case", f => f
                .OfType(nameof(LocalizationSetContentPickerField))
                .WithDisplayName("Case")
                .WithPosition(position)
                .WithSettings(new LocalizationSetContentPickerFieldSettings() { DisplayedContentTypes = new string[] { "Case" } })
            );
        }

        public static ContentPartDefinitionBuilder WithHtmlField(this ContentPartDefinitionBuilder p, string name, string displayName, string hint, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(HtmlField))
                .WithEditor("Wysiwyg")
                .WithDisplayName(displayName)
                .WithPosition(position)
                .WithSettings(new HtmlFieldSettings() { Hint = hint })
            );
        }

        public static ContentPartDefinitionBuilder WithTextField(this ContentPartDefinitionBuilder p, string name, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(name)
                .WithPosition(position)
            );
        }

        public static ContentPartDefinitionBuilder WithTextField(this ContentPartDefinitionBuilder p, string name, string displayName, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(displayName)
                .WithPosition(position)
            );
        }
        public static ContentPartDefinitionBuilder WithTextField(this ContentPartDefinitionBuilder p, string name, string displayName, string position, TextFieldSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(displayName)
                .WithPosition(position)
                .WithSettings(settings)
            );
        }

        public static ContentPartDefinitionBuilder WithTextField(this ContentPartDefinitionBuilder p, string name, string displayName, string editor, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(displayName)
                .WithEditor(editor)
                .WithPosition(position)
            );
        }

        public static ContentPartDefinitionBuilder WithNumericField(this ContentPartDefinitionBuilder p, string name, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(NumericField))
                .WithDisplayName(name)
                .WithPosition(position)
            );
        }

        public static ContentPartDefinitionBuilder WithNumericField(this ContentPartDefinitionBuilder p, string name, string position, NumericFieldSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(NumericField))
                .WithDisplayName(name)
                .WithPosition(position)
                .WithSettings(settings)
            );
        }

        public static ContentPartDefinitionBuilder WithSwitchBooleanField(this ContentPartDefinitionBuilder p, string name, string displayName, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(BooleanField))
                .WithEditor("Switch")
                .WithDisplayName(displayName)
                .WithPosition(position)
            );
        }

        public static ContentPartDefinitionBuilder WithSwitchBooleanField(this ContentPartDefinitionBuilder p, string name, string displayName, string position, BooleanFieldSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(BooleanField))
                .WithEditor("Switch")
                .WithDisplayName(displayName)
                .WithPosition(position)
                .WithSettings(settings)
            );
        }

        public static ContentTypeDefinitionBuilder WithAdvancedFlowPart(this ContentTypeDefinitionBuilder t, string position)
        {
            return t.WithPart("AdvancedFlowPart", p => p.WithPosition(position));
        }
    }
}