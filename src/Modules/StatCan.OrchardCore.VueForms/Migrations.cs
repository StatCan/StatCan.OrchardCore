using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.VueForms
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            // Form
            _contentDefinitionManager.AlterPartDefinition("VueForm", part => part
                .WithBooleanField("Enabled", "Form enabled", "1",
                    new BooleanFieldSettings()
                    {
                        Hint = "The form accepts submissions if is enabled"
                    }
                )
                .WithField("DisabledHtml", f => f
                    .OfType(nameof(HtmlField))
                    .WithDisplayName("Disabled Html")
                    .WithSettings(new HtmlFieldSettings() { Hint = "Html displayed when someone tries to render a disabled form.", SanitizeHtml = true})
                    .WithPosition("2")
                )
                .WithTextField("SuccessMessage", "Success Message", "3", new TextFieldSettings()
                {
                    Hint = "(optional) The message returned to the client if validation passed and no redirect has been set. With liquid support."
                })
                .WithTextField("ErrorMessage", "Error Message", "4", new TextFieldSettings()
                {
                    Hint = "(optional) The message to display if a server error occured in the ajax request. With liquid support."
                })
                .Attachable()
                .WithDescription("Turns your content items into a vue form."));

            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("ClientInit", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Client Init")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs client side to set various options for your form (such as setup the VeeValidate locales). With liquid support." })
                    .WithPosition("0")
                    .WithEditor("CodeMirrorJS")
                )
                .WithField("VueComponentScript", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Vue Component Script")
                   .WithSettings(new TextFieldSettings() {  Required = true, Hint = "VueJS Component script. Write the JS object that represents the script part of the vue component without a return statement. With liquid support." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorLiquid")
               )
                .WithField("OnValidation", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Validation")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side to validate your form."})
                    .WithPosition("2")
                    .WithEditor("CodeMirrorJS")
                )
                .WithField("OnSubmitted", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Submitted")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side after when the form is valid, before the workflow event is triggerred." })
                    .WithPosition("3")
                    .WithEditor("CodeMirrorJS")
                )
                .Attachable()
                .WithDescription("Script fields for AjaxForm"));

            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                .Draftable()
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueForm", p => p.WithPosition("1"))
                .WithPart("FlowPart", p => p.WithPosition("2"))
                .WithPart("VueFormScripts", p => p.WithPosition("3")));

            // Vue component
            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
               .WithField("Template", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Template")
                   .WithSettings(new TextFieldSettings() { Required = true, Hint = "VueJS Component template. Need to return a single node. Vuetify(https://vuetifyjs.com/en/components/forms) and  VeeValidate(https://logaretm.github.io/vee-validate/guide/basics.html) librairies are loaded by default. With liquid support." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorLiquid")
            ));

            _contentDefinitionManager.AlterTypeDefinition("VueComponent", type => type
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueComponent", p => p.WithPosition("1"))
                .Stereotype("Widget"));

            AddVueFormReference();

            return 3;
        }

        public int UpdateFrom1()
        {
            AddVueFormReference();

            return 2;
        }

        public int UpdateFrom2()
        {
            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("Vue Component Script", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("VueComponentScript")
                   .WithSettings(new TextFieldSettings() {  Required = true, Hint = "VueJS Component script. Write the JS object that represents the script part of the vue component without a return statement. With liquid support." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorLiquid")
               )
                .WithField("OnValidation", f => f
                    .WithPosition("2")
                )
                .WithField("OnSubmitted", f => f
                    .WithPosition("3")
                )
                .Attachable()
                .WithDescription("Script fields for AjaxForm"));

            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
                .RemoveField("Script")
            );
            return 3;
        }

        public void AddVueFormReference()
        {
             // VueFormReference is a widget meant
            _contentDefinitionManager.AlterTypeDefinition("VueFormReference", type => type
                .DisplayedAs("Vue Form Reference")
                .Stereotype("Widget")
                .WithPart("VueFormReference", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        RenderTitle = false
                    })
                )
            );
            _contentDefinitionManager.AlterPartDefinition("VueFormReference", part => part
                .WithField("FormReference", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("FormReference")
                    .WithSettings(new ContentPickerFieldSettings
                    {
                        DisplayedContentTypes = new[] { "VueForm" }
                    })
                )
            );
        }
    }

    public class LocalizationMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public LocalizationMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            // Weld the LocalizedText part
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                 .WithPart("LocalizedTextPart", p => p.WithPosition("4"))
            );

            return 1;
        }
    }
}
