using Etch.OrchardCore.ContentPermissions.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Models;
using OrchardCore.Markdown.Fields;
using OrchardCore.Markdown.Models;
using OrchardCore.Markdown.Settings;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;

namespace StatCan.OrchardCore.Extensions
{
    /// <summary>
    /// Content Type / Part builder extensions to simplify migrations
    /// </summary>
    public static class BuilderExtensions
    {
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

        public static ContentPartDefinitionBuilder WithMarkdownField(this ContentPartDefinitionBuilder p, string name, string displayName, string hint, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(MarkdownField))
                .WithEditor("Wysiwyg")
                .WithDisplayName(displayName)
                .WithPosition(position)
                .WithSettings(new MarkdownFieldSettings() { Hint = hint })
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
        public static ContentPartDefinitionBuilder WithTextFieldPredefinedList(this ContentPartDefinitionBuilder p, string name, string displayName, string position, TextFieldPredefinedListEditorSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(displayName)
                .WithEditor("PredefinedList")
                .WithPosition(position)
                .WithSettings(settings)
            );
        }
        public static ContentPartDefinitionBuilder WithTextFieldPredefinedList(this ContentPartDefinitionBuilder p, string name, string displayName, string position, string hint, TextFieldPredefinedListEditorSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(TextField))
                .WithDisplayName(displayName)
                .WithEditor("PredefinedList")
                .WithPosition(position)
                .WithSettings(new TextFieldSettings() { Hint = hint })
                .WithSettings(settings)
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
        public static ContentPartDefinitionBuilder WithNumericField(this ContentPartDefinitionBuilder p, string name, string displayName, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(NumericField))
                .WithDisplayName(displayName)
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
        public static ContentPartDefinitionBuilder WithBooleanField(this ContentPartDefinitionBuilder p, string name, string displayName, string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(BooleanField))
                .WithDisplayName(displayName)
                .WithPosition(position)
            );
        }
        public static ContentPartDefinitionBuilder WithBooleanField(this ContentPartDefinitionBuilder p, string name, string displayName, string position, BooleanFieldSettings settings)
        {
            return p.WithField(name, f => f
                .OfType(nameof(BooleanField))
                .WithDisplayName(displayName)
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

        public static ContentPartDefinitionBuilder WithMultiValueTextField(this ContentPartDefinitionBuilder p, string name, string displayName, string position, ListValueOption[] options, MultiValueEditorOption editor = MultiValueEditorOption.Checkbox)
        {
            return p.WithField(name, field => field
               .OfType("MultiValueTextField")
               .WithDisplayName(displayName)
               .WithEditor("PredefinedList")
               .WithPosition(position)
               .WithSettings(new MultiValueTextFieldEditorSettings
               {
                   Options = options,
                   Editor = editor
               })
            );
        }

        public static ContentTypeDefinitionBuilder WithHtmlBody(this ContentTypeDefinitionBuilder t, string position)
        {
            return t.WithPart(nameof(HtmlBodyPart), p => {
                p.WithPosition(position);
                p.WithDisplayName("Html Body");
                p.WithEditor("Wysiwyg");
            });
        }

        public static ContentTypeDefinitionBuilder WithMarkdownBody(this ContentTypeDefinitionBuilder t, string position)
        {
            return t.WithPart(nameof(MarkdownBodyPart), p => {
                p.WithPosition(position);
                p.WithDisplayName("Markdown Body");
                p.WithEditor("Wysiwyg");
            });
        }

        public static ContentTypeDefinitionBuilder WithFlow(this ContentTypeDefinitionBuilder t, string position, string[] containedContentTypes = null)
        {
            return t.WithPart(nameof(FlowPart), p => {
                p.WithPosition(position);
                if (containedContentTypes != null)
                {
                    p.WithSettings(new FlowPartSettings() { ContainedContentTypes = containedContentTypes });
                }
            });
        }

        public static ContentTypeDefinitionBuilder WithContentPermission(this ContentTypeDefinitionBuilder t, string position, string redirectUrl = null)
        {
            return t.WithPart(nameof(ContentPermissionsPart), p => {
                p.WithPosition(position);
                if (redirectUrl != null)
                {
                    p.WithSettings(new ContentPermissionsPartSettings() { RedirectUrl = redirectUrl });
                }
            });
        }

        public static void CreateBasicWidget(this IContentDefinitionManager manager, string name)
        {
            manager.AlterPartDefinition(name, p => p.WithDisplayName(name));
            manager.AlterTypeDefinition(name, t => t.Stereotype("Widget")
               .WithPart(name, p => p.WithPosition("0"))
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
                .WithSettings(new TitlePartSettings() { Options = options, Pattern = pattern })
            );
        }
    }
}
