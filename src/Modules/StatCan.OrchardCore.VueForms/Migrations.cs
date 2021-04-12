using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;

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
                .WithField("RenderAs", field => field
                    .OfType("TextField")
                    .WithDisplayName("Render as")
                    .WithEditor("PredefinedList")
                    .WithPosition("2")
                    .WithSettings(new TextFieldPredefinedListEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name= "Vue Component",
                            Value= ""
                            }, new ListValueOption() {
                            Name= "Vue App",
                            Value= "VueApp"
                            }, new ListValueOption() {
                            Name= "Vuetify App",
                            Value= "VuetifyApp"
                            } },
                        Editor = EditorOption.Dropdown,
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Render this form as a Vue component, a standalone vue app or a vuetify app (wrapping with v-app)"
                    })
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
                .WithDescription("Turns your content items into a vue form."));

            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("ClientInit", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Client Init")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs client side to set various options for your form (such as setup the VeeValidate locales). With liquid support." })
                    .WithPosition("0")
                    .WithEditor("CodeMirrorJS")
                )
                .WithField("ComponentOptions", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Component Options object")
                   .WithSettings(new TextFieldSettings() {  Required = true, Hint = "The form's vue component options object. The component's data object is sent to the server. With liquid support." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorJS")
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
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side after form validation has passed, before the workflow event is triggerred." })
                    .WithPosition("3")
                    .WithEditor("CodeMirrorJS")
                )
                .Attachable()
                .WithDescription("Script fields for AjaxForm"));

            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                .Draftable().Securable()
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueForm", p => p.WithPosition("1"))
                .WithPart("FlowPart", p => p.WithPosition("2"))
                .WithPart("VueFormScripts", p => p.WithPosition("3"))
                .WithPart("ContentPermissionsPart", p => p.WithPosition("4")));

            // Vue component
            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
               .WithField("Template", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Template")
                   .WithSettings(new TextFieldSettings() { Required = true, Hint = "VueJS Component template. Need to return a single node. The VeeValidate(https://logaretm.github.io/vee-validate/guide/basics.html) library is used for client / server side validaiton support. With liquid support." })
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
                .WithField("ComponentOptions", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Component Options object")
                   .WithSettings(new TextFieldSettings() {  Required = true, Hint = "The form's vue component options object. With liquid support." })
                   .WithPosition("1")
                   .WithEditor("CodeMirrorJS")
               )
            );

            _contentDefinitionManager.AlterPartDefinition("VueComponent", part => part
                .RemoveField("Script")
            );


             _contentDefinitionManager.AlterPartDefinition("VueForm", part => part
                .RemoveField("ErrorMessage")
                .WithField("RenderAs", field => field
                    .OfType("TextField")
                    .WithDisplayName("Render as")
                    .WithEditor("PredefinedList")
                    .WithPosition("2")
                    .WithSettings(new TextFieldPredefinedListEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name= "Vue Component",
                            Value= ""
                            }, new ListValueOption() {
                            Name= "Vue App",
                            Value= "VueApp"
                            }, new ListValueOption() {
                            Name= "Vuetify App",
                            Value= "VuetifyApp"
                            } },
                        Editor = EditorOption.Dropdown,
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Render this form as a Vue component, a standalone vue app or a vuetify app (wrapping with v-app)"
                    })
                ));
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type.Securable());
            return 3;
        }

        public int updateFrom3()
        {
            _contentDefinitionManager.AlterTypeDefinition("VTextField", type => type
                .DisplayedAs("VTextField")
                .Stereotype("Widget")
                .WithPart("VTextField", part => part
                    .WithPosition("1")
                )
                .WithTitlePart("0", TitlePartOptions.GeneratedDisabled, "{{ Model.ContentItem.Content.VExpansionPanel.Header.Text }}")
            );

            _contentDefinitionManager.AlterPartDefinition("VTextField", part => part
                .WithField("Model", field => field
                    .OfType("TextField")
                    .WithDisplayName("Model")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "The model for this field.",
                        Required = true,
                    })
                )
                .WithField("Label", field => field
                    .OfType("TextField")
                    .WithDisplayName("Label")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Sets input label.",
                        Required = true,
                    })
                )
                .WithField("Value", field => field
                    .OfType("TextField")
                    .WithDisplayName("Value")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "The input’s value.",
                    })
                )
                .WithField("Placeholder", field => field
                    .OfType("TextField")
                    .WithDisplayName("Placeholder")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Sets the input’s placeholder text.",
                    })
                )
                .WithField("Hint", field => field
                    .OfType("TextField")
                    .WithDisplayName("Hint")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Hint text.",
                    })
                )
                .WithField("Prefix", field => field
                    .OfType("TextField")
                    .WithDisplayName("Prefix")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Displays prefix text.",
                    })
                )
                .WithField("Suffix", field => field
                    .OfType("TextField")
                    .WithDisplayName("Suffix")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Displays suffix text.",
                    })
                )
                .WithField("AppendIcon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Append Icon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Appends an icon to the component.",
                    })
                )
                .WithField("AppendOuterIcon", field => field
                    .OfType("TextField")
                    .WithDisplayName("AppendOuterIcon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Appends an icon to the outside the component’s input.",
                    })
                )
                .WithField("PrependIcon", field => field
                    .OfType("TextField")
                    .WithDisplayName("PrependIcon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Prepends an icon to the component.",
                    })
                )
                .WithField("PrependInnerIcon", field => field
                    .OfType("TextField")
                    .WithDisplayName("PrependInnerIcon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Prepends an icon inside the component’s input.",
                    })
                )
                .WithField("Color", field => field
                    .OfType("TextField")
                    .WithDisplayName("Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("BackgroundColor", field => field
                    .OfType("TextField")
                    .WithDisplayName("Background Color")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applies specified color to the control background - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)).",
                    })
                )
                .WithField("ClearIcon", field => field
                    .OfType("TextField")
                    .WithDisplayName("Clear Icon")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Applied when using clearable and the input is dirty.",
                    })
                )
                .WithField("Props", field => field
                    .OfType("MultiValueTextField")
                    .WithDisplayName("Props")
                    .WithEditor("PredefinedList")
                    .WithSettings(new MultiValueTextFieldEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name = "Autofocus",
                            Value = "autofocus"
                        }, new ListValueOption() {
                            Name = "Clearable",
                            Value = "clearable"
                        }, new ListValueOption() {
                            Name = "Dark",
                            Value = "dark"
                        }, new ListValueOption() {
                            Name = "Dense",
                            Value = "dense"
                        }, new ListValueOption() {
                            Name = "Disabled",
                            Value = "disabled"
                        }, new ListValueOption() {
                            Name = "Error",
                            Value = "error"
                        }, new ListValueOption() {
                            Name = "Flat",
                            Value = "flat"
                        }, new ListValueOption() {
                            Name = "Filled",
                            Value = "full-width"
                        }, new ListValueOption() {
                            Name = "Light",
                            Value = "light"
                        }, new ListValueOption() {
                            Name = "Loading",
                            Value = "loading"
                        }, new ListValueOption() {
                            Name = "Outlined",
                            Value = "outlined"
                        }, new ListValueOption() {
                            Name = "Persistent Hint",
                            Value = "persistent-hint"
                        }, new ListValueOption() {
                            Name = "Read-only",
                            Value = "readonly"
                        }, new ListValueOption() {
                            Name = "Reverse",
                            Value = "reverse"
                        }, new ListValueOption() {
                            Name = "Rounded",
                            Value = "rounded"
                        }, new ListValueOption() {
                            Name = "Shaped",
                            Value = "shaped"
                        }, new ListValueOption() {
                            Name = "Single Line",
                            Value = "single-line"
                        }, new ListValueOption() {
                            Name = "Solo",
                            Value = "solo"
                        }, new ListValueOption() {
                            Name = "Solo Inverted",
                            Value = "solo-inverted"
                        }, new ListValueOption() {
                            Name = "Success",
                            Value = "success"
                        }, new ListValueOption() {
                            Name = "Validate on Blur",
                            Value = "validate-on-blur"
                        } },
                    })
                )
            );
            return 4;
        }

        public void AddVueFormReference()
        {
            _contentDefinitionManager.AlterTypeDefinition("VueFormReference", type => type
                .DisplayedAs("Vue Form Reference")
                .Stereotype("Widget")
                .WithPart("VueFormReference", part => part
                    .WithPosition("0")
                )
            );
            _contentDefinitionManager.AlterPartDefinition("VueFormReference", part => part
                .WithField("FormReference", field => field
                    .OfType("ContentPickerField")
                    .WithDisplayName("Form Reference")
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
