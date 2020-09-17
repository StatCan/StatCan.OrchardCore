using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
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
                .WithTextField("SuccessMessage", "Success Message", "2", new TextFieldSettings()
                {
                    Hint = "(optional) The message returned to the client if validation passed and no redirect has been set. With liquid support."
                })
                .WithTextField("ErrorMessage", "Error Message", "3", new TextFieldSettings()
                {
                    Hint = "(optional) The message to display if a server error occured in the ajax request. With liquid support."
                })
                .Attachable()
                .WithDescription("Turns your content items into a vue form."));

            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("ClientInit", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("ClientInit")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs client side to set various options for your form (such as setup the VeeValidate locales)" })
                    .WithPosition("0")
                    .WithEditor("CodeMirrorJS")
                )
                .WithField("OnValidation", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("OnValidation")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side to validate your form."})
                    .WithPosition("1")
                    .WithEditor("CodeMirrorJS")
                )
                .WithField("OnSubmitted", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("OnSubmitted")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side after when the form is valid, before the workflow event is triggerred." })
                    .WithPosition("2")
                    .WithEditor("CodeMirrorJS")
                )

                .Attachable()
                .WithDescription("Script fields for AjaxForm"));

            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                .Draftable()
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueForm", p => p.WithPosition("2"))
                .WithPart("FlowPart", p => p.WithPosition("3"))
                .WithPart("VueFormScripts", p => p.WithPosition("4")));

            // Vue component
            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
               .WithField("Template", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Template")
                   .WithSettings(new TextFieldSettings() { Required= true, Hint = "VueJS Component template. Need to return a single node. Vuetify(https://vuetifyjs.com/en/components/forms) and  VeeValidate(https://logaretm.github.io/vee-validate/guide/basics.html) librairies are loaded by default." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorLiquid")
               )
               .WithField("Script", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Script")
                   .WithSettings(new TextFieldSettings() { Hint = "VueJS Component script. Write the JS object that represents the script part of the vue component without a return statement." })
                   .WithPosition("2")
                   .WithEditor("CodeMirrorLiquid")
               ));

            _contentDefinitionManager.AlterTypeDefinition("VueComponent", type => type
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueComponent", p => p.WithPosition("1"))
                .Stereotype("Widget"));


            return 1;
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
                 .WithPart("LocalizedText", p => p.WithPosition("1"))
            );
            return 1;
        }
    }
}