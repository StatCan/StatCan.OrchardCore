using Etch.OrchardCore.ContentPermissions.Models;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Builders;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Flows.Models;

namespace StatCan.OrchardCore.Extensions
{
    /// <summary>
    /// Content Type / Part builder extensions to simplify migrations
    /// </summary>
    public static class BuilderExtensions
    {
        public static ContentPartDefinitionBuilder WithHtmlField(this ContentPartDefinitionBuilder p, string name, string displayName, string hint,  string position)
        {
            return p.WithField(name, f => f
                .OfType(nameof(HtmlField))
                .WithEditor("Wysiwyg")
                .WithDisplayName(displayName)
                .WithPosition(position)
                .WithSettings(new HtmlFieldSettings(){Hint = hint})
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

        public static ContentTypeDefinitionBuilder WithFlow(this ContentTypeDefinitionBuilder t, string position, string[] containedContentTypes = null)
        {
            return t.WithPart(nameof(FlowPart), p => {
                p.WithPosition(position);
                if(containedContentTypes != null)
                {
                    p.WithSettings(new FlowPartSettings() { ContainedContentTypes = containedContentTypes });
                }
            });
        }

        public static ContentTypeDefinitionBuilder WithContentPermission(this ContentTypeDefinitionBuilder t, string position, string redirectUrl = null)
        {
            return t.WithPart(nameof(ContentPermissionsPart), p => {
                p.WithPosition(position);
                if(redirectUrl != null)
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
    }
}