using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.ContentsExtensions;

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
                .WithPart("VueForm", p => p.WithPosition("1"))
                .WithPart("FlowPart", p => p.WithPosition("2"))
                .WithPart("VueFormScripts", p => p.WithPosition("3")));

            // Vue component
            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
               .WithField("Template", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Template")
                   .WithSettings(new TextFieldSettings() { Required= true, Hint = "VueJS Component template. Need to return a single node. <a target=\"_blank\" href=\"https://vuetifyjs.com/en/components/forms\" >Vuetify</a> and <a target=\"_blank\" href=\"https://logaretm.github.io/vee-validate/guide/basics.html\" >VeeValidate</a> librairies are loaded by default. " })
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
               .WithPart("VueComponent", p => p.WithPosition("0"))
               .Stereotype("Widget"));


            return 1;
        }

        //// Fieldset
        //_contentDefinitionManager.AlterTypeDefinition("FormFieldset", type => type
        //   .WithPart("TitlePart", p => p.WithPosition("0"))
        //   .WithPart("FlowPart", p => p.WithPosition("1"))
        //   .Stereotype("Widget"));

        //// Text Input
        //_contentDefinitionManager.AlterPartDefinition("FormInput", p => p
        //    .WithTextField("Name", "Name", "0", new TextFieldSettings() { Required = true })
        //    .WithField("Type", f => f
        //        .OfType(nameof(TextField))
        //        .WithDisplayName("Type")
        //        .WithPosition("1")
        //        .WithEditor("PredefinedList")
        //        .WithSettings(new TextFieldPredefinedListEditorSettings()
        //        {
        //            Editor = EditorOption.Dropdown,
        //            DefaultValue = "text",
        //            Options = new ListValueOption[] {
        //                new ListValueOption(){Name = "text", Value = "text"},
        //                new ListValueOption(){Name = "email", Value = "email"},
        //                new ListValueOption(){Name = "url", Value = "url"},
        //                new ListValueOption(){Name = "number", Value = "number"},
        //                new ListValueOption(){Name = "tel", Value = "tel"},
        //                new ListValueOption(){Name = "date", Value = "date"},
        //                new ListValueOption(){Name = "password", Value = "password"},
        //                new ListValueOption(){Name = "color", Value = "color"}
        //            }
        //        })
        //    )
        //    // text
        //    .WithTextField("Label", "Label", "2", new TextFieldSettings()
        //    {
        //        Required = true,
        //        Hint = "(required) Input label. With liquid support."
        //    })
        //    .WithTextField("Placeholder", "Placeholder", "3", new TextFieldSettings()
        //    {
        //        Hint = "(optional) Input placeholder. With liquid support."
        //    })
        //    .WithTextField("HelpText", "Help Text", "4", new TextFieldSettings()
        //    {
        //        Hint = "(optional) Help text"
        //    })
        //    .Attachable()
        //    .WithDescription("Basic fields for form input")
        //);
        //_contentDefinitionManager.AlterPartDefinition("FormInputStyle", p => p
        //    .WithTextField("WrapperClass", "WrapperClass", "0", new TextFieldSettings()
        //    {
        //        Hint = "(optional) CSS class applied to the wrapper div. 'form-group' by default"
        //    })
        //    .WithTextField("LabelClass", "LabelClass", "1", new TextFieldSettings()
        //    {
        //        Hint = "(optional) CSS class applied to the label element. Blank by default"
        //    })
        //    .WithTextField("InputClass", "InputClass", "2", new TextFieldSettings()
        //    {
        //        Hint = "(optional) CSS class applied to the input element. 'form-control' by default"
        //    })
        //    .Attachable()
        //    .WithDescription("Wrapper, Label and Input class fields for form inputs")
        //);
        //_contentDefinitionManager.AlterPartDefinition("FormRequiredValidation", p => p
        //    .WithBooleanField("Required", "Required", "0",
        //        new BooleanFieldSettings()
        //        {
        //            Hint = "Is this input required?"
        //        }
        //    )
        //    .WithTextField("RequiredText", "Required Error Message", "1", new TextFieldSettings()
        //    {
        //        Hint = "(optional) Message displayed when the input is required and is not filled. With liquid support. With liquid support."
        //    })
        //    .Attachable()
        //    .WithDescription("AjaxForm, required validation fields")
        //);
        //_contentDefinitionManager.AlterTypeDefinition("FormInput", type => type
        //   .WithPart(nameof(TitlePart), p => p
        //       .WithPosition("0")
        //       .WithSettings(new TitlePartSettings()
        //       {
        //           Options = TitlePartOptions.GeneratedHidden,
        //           Pattern = "{{ ContentItem.Content.FormInput.Name.Text }}"
        //       })
        //   )
        //   .WithPart("FormInput", p => p.WithPosition("1"))
        //   .WithPart("FormInputStyle", p => p.WithPosition("2"))
        //   .WithPart("FormRequiredValidation", p => p.WithPosition("3"))
        //   .Stereotype("Widget")
        //);
        //// Button
        //_contentDefinitionManager.AlterPartDefinition("FormButton", p => p
        //    .WithTextField("Name", "Name", "0", new TextFieldSettings() { Required = true })
        //    .WithField("Type", f => f
        //        .OfType(nameof(TextField))
        //        .WithDisplayName("Type")
        //        .WithPosition("1")
        //        .WithEditor("PredefinedList")
        //        .WithSettings(new TextFieldPredefinedListEditorSettings()
        //        {
        //            Editor = EditorOption.Dropdown,
        //            DefaultValue = "text",
        //            Options = new ListValueOption[] {
        //                new ListValueOption(){Name = "submit", Value = "submit"},
        //                new ListValueOption(){Name = "button", Value = "button"}
        //            }
        //        })
        //    )
        //    // text
        //    .WithTextField("Label", "Label", "2", new TextFieldSettings()
        //    {
        //        Required = true,
        //        Hint = "(required) Button text. With liquid support."
        //    })
        //    .WithTextField("CssClass", "Css Class", "3", new TextFieldSettings()
        //    {
        //        Hint = "(optional) CSS class applied to the button. 'btn btn-primary form-group' by default"
        //    })
        //    .WithBooleanField("DisableOnSubmit", "Disable on Submit", "4",
        //        new BooleanFieldSettings()
        //        {
        //            Hint = "Disables the button when submitting the form"
        //        }
        //    )
        //);
        //_contentDefinitionManager.AlterTypeDefinition("FormButton", type => type
        //   .WithPart(nameof(TitlePart), p => p
        //       .WithPosition("0")
        //       .WithSettings(new TitlePartSettings()
        //       {
        //           Options = TitlePartOptions.GeneratedHidden,
        //           Pattern = "{{ ContentItem.Content.FormButton.Name.Text }}"
        //       })
        //   )
        //   .WithPart("FormButton")
        //   .Stereotype("Widget"));

    }
}