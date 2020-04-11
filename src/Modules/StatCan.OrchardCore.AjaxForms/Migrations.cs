using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.ContentsExtensions;

namespace StatCan.OrchardCore.AjaxForms
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
            _contentDefinitionManager.AlterPartDefinition("AjaxForm", part => part
                .WithBooleanField("Enabled","Form enabled", "1",
                    new BooleanFieldSettings(){
                        Hint = "The form accepts submissions if is enabled"}
                )
                .WithBooleanField("DebugValues","Debug form values", "2",
                    new BooleanFieldSettings(){
                        Hint = "Displays json to be submitted to the forms controller in the frontend."}
                )
                .Attachable()
                .WithDescription("Turns your content item into an ajax form."));

            _contentDefinitionManager.AlterTypeDefinition("AjaxForm", type => type
                .Draftable().Versionable()
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("AjaxForm")
                .WithPart("FlowPart"));

            // Fieldset
             _contentDefinitionManager.AlterTypeDefinition("FormFieldset", type => type
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("FlowPart")
                .Stereotype("Widget"));

            // Text Input
            _contentDefinitionManager.AlterPartDefinition("FormTextInput", p => p
                .WithTextField("Name", "Name", "0", new TextFieldSettings(){ Required = true })
                .WithField("Type", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Type")
                    .WithPosition("1")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = EditorOption.Dropdown,
                        DefaultValue = "text",
                        Options = new ListValueOption[] {
                            new ListValueOption(){Name = "text", Value = "text"},
                            new ListValueOption(){Name = "email", Value = "email"},
                            new ListValueOption(){Name = "url", Value = "url"},
                            new ListValueOption(){Name = "number", Value = "number"},
                            new ListValueOption(){Name = "tel", Value = "tel"},
                            new ListValueOption(){Name = "date", Value = "date"},
                            new ListValueOption(){Name = "password", Value = "password"},
                            new ListValueOption(){Name = "color", Value = "color"}
                        }
                    })
                )
                // text
                .WithTextField("Label", "Label", "2", new TextFieldSettings(){
                    Required = true,
                    Hint = "(required) Input label"
                })
                .WithTextField("Placeholder", "Placeholder", "3", new TextFieldSettings(){
                    Hint = "(optional) Input placeholder"
                })
                .WithTextField("HelpText", "Help Text", "4", new TextFieldSettings(){
                    Hint = "(optional) Help text"
                })
                // styles
                .WithTextField("LabelClass", "LabelClass", "5", new TextFieldSettings(){
                    Hint = "(optional) CSS class applied to the label element. Blank by default"
                })
                .WithTextField("InputClass", "InputClass", "6", new TextFieldSettings(){
                    Hint = "(optional) CSS class applied to the input element. 'form-control' by default"
                })
                .WithTextField("WrapperClass", "WrapperClass", "7", new TextFieldSettings(){
                    Hint = "(optional) CSS class applied to the input element. 'form-group' by default"
                })
                // validation
                .WithTextField("ValidText", "Valid Message", "8", new TextFieldSettings(){
                    Hint = "(optional) Message displayed when the input is valid."
                })
                .WithTextField("InvalidText", "Invalid Message", "9", new TextFieldSettings(){
                    Hint = "(optional) Message displayed when the input is invalid."
                })
                .WithBooleanField("Required","Required", "10",
                    new BooleanFieldSettings(){
                        Hint = "Is this input required?"}
                )
            );
             _contentDefinitionManager.AlterTypeDefinition("FormTextInput", type => type
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.FromTextInput.Name.Text }}" })
                )
                .WithPart("FormTextInput")
                .Stereotype("Widget"));

            // Button
            _contentDefinitionManager.AlterPartDefinition("FormButton", p => p
                .WithTextField("Name", "Name", "0", new TextFieldSettings(){ Required = true })
                .WithField("Type", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Type")
                    .WithPosition("1")
                    .WithEditor("PredefinedList")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = EditorOption.Dropdown,
                        DefaultValue = "text",
                        Options = new ListValueOption[] {
                            new ListValueOption(){Name = "submit", Value = "submit"},
                            new ListValueOption(){Name = "button", Value = "button"}
                        }
                    })
                )
                // text
                .WithTextField("Label", "Label", "2", new TextFieldSettings(){
                    Required = true,
                    Hint = "(required) Button text"
                })
                .WithTextField("CssClass", "Css Class", "3", new TextFieldSettings(){
                    Hint = "(optional) CSS class applied to the button. 'btn btn-primary form-group' by default"
                })
                .WithBooleanField("DisableOnSubmit","Disable on Submit", "10",
                    new BooleanFieldSettings(){
                        Hint = "Disables the button when submitting the form"}
                )
            );
             _contentDefinitionManager.AlterTypeDefinition("FormButton", type => type
                .WithPart(nameof(TitlePart), p => p
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings() {
                        Options = TitlePartOptions.GeneratedHidden,
                        Pattern = "{{ ContentItem.Content.FormButton.Name.Text }}" })
                )
                .WithPart("FormButton")
                .Stereotype("Widget"));

            return 1;
        }

    }
}