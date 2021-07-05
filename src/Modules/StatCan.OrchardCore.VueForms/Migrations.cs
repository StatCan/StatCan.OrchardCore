using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings;
using OrchardCore.ContentManagement;
using YesSql;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Flows.Models;
using System.Text;
using StatCan.OrchardCore.VueForms.Models;

namespace StatCan.OrchardCore.VueForms
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ISession _session;

        public Migrations(IContentDefinitionManager contentDefinitionManager, ISession session)
        {
            _contentDefinitionManager = contentDefinitionManager;
            _session = session;
        }

        public int Create()
        {
            // Form
            _contentDefinitionManager.AlterPartDefinition("VueForm", part => part
                .WithField("RenderAs", field => field
                    .OfType("TextField")
                    .WithDisplayName("Render as")
                    .WithEditor("PredefinedList")
                    .WithPosition("0")
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
                    .WithSettings(new TextFieldSettings()
                    {
                        Hint = "Render this form as a Vue component, a standalone vue app or a vuetify app (wrapping with v-app)"
                    })
                )
                .WithBooleanField("Disabled", "Disable the form", "1",
                    new BooleanFieldSettings()
                    {
                        Hint = "If disabled, the Disabled Html field will be rendered in place of the form "
                    }
                )
                .WithBooleanField("Debug", "Debug", "2",
                    new BooleanFieldSettings()
                    {
                        Hint = "Debug mode is meant to be used when developping VueForms and will return additional debug information to the client along with the response"
                    }
                )
                .WithTextField("SuccessMessage", "Success Message", "3", new TextFieldSettings()
                {
                    Hint = "Message returned to the client when everything completed successfully"
                })
                .WithField("DisabledHtml", f => f
                    .OfType(nameof(HtmlField))
                    .WithDisplayName("Disabled Html")
                    .WithSettings(new HtmlFieldSettings() { Hint = "Html displayed when someone tries to render a disabled form.", SanitizeHtml = true })
                    .WithPosition("2")
                )
                .WithField("Template", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Template")
                    .WithSettings(new TextFieldSettings() { Required = true, Hint = "The form's template. VeeValidate v3(https://logaretm.github.io/vee-validate/guide/basics.html) is used for client side validation support. With liquid support." })
                    .WithPosition("3")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"html\"}"
                        })
                )
                .WithDescription("Turns your content items into a vue form."));

            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("ClientInit", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Client Init")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs client side to set various options for your form (such as setup the VeeValidate locales). Liquid is evaluated before passing the script to the client." })
                    .WithPosition("0")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\", \"renderValidationDecorations\": \"off\"}"
                        })
                )
                .WithField("ComponentOptions", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Component Options object")
                   .WithSettings(new TextFieldSettings() { Hint = "The form's vue component options object (https://vuejs.org/v2/api/#Options-Data). The component's data object is sent to the server. Liquid is evaluated before passing the script to the client." })
                   .WithPosition("1")
                .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\", \"renderValidationDecorations\": \"off\"}"
                        })
                )
                .WithField("OnValidation", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Validation")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side to validate your form." })
                    .WithPosition("2")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\"}"
                        })
                )
                .WithField("OnSubmitted", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Submitted")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side after form validation has passed, before the workflow event is triggerred." })
                    .WithPosition("3")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\"}"
                        })
                )
                .Attachable()
                .WithDescription("Script fields for VueForms"));

            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                .Draftable().Securable().Versionable()
                .WithPart("TitlePart", p => p.WithPosition("0"))
                .WithPart("VueForm", p => p.WithPosition("1"))
                .WithPart("AliasPart", p => p.WithPosition("2"))
                .WithPart("VueFormScripts", p => p.WithPosition("3"))
                .WithPart("ContentPermissionsPart", p => p.WithPosition("4")));

            AddVueFormReference();
            return 7;
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
                   .WithSettings(new TextFieldSettings() { Required = true, Hint = "The form's vue component options object. With liquid support." })
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
                    .WithSettings(new TextFieldSettings()
                    {
                        Hint = "Render this form as a Vue component, a standalone vue app or a vuetify app (wrapping with v-app)"
                    })
                )
            );
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type.Securable());
            return 3;
        }

        public int UpdateFrom3()
        {
            AddVTextField();
            return 4;
        }

        public int UpdateFrom4()
        {
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type =>
                type.WithPart("ContentPermissionsPart", p => p.WithPosition("4"))
            );
            return 5;
        }
        public int UpdateFrom5()
        {
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type.Versionable());
            return 6;
        }
        public async Task<int> UpdateFrom6Async()
        {
            _contentDefinitionManager.AlterPartDefinition("VueForm", part => part
                .RemoveField("Enabled")
                .WithTextField("SuccessMessage", "Success Message", "3", new TextFieldSettings()
                {
                    Hint = "Message returned to the client when everything completed successfully"
                })
                .WithField("RenderAs", field => field
                    .OfType("TextField")
                    .WithDisplayName("Render as")
                    .WithEditor("PredefinedList")
                    .WithPosition("0")
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
                    .WithSettings(new TextFieldSettings()
                    {
                        Hint = "Render this form as a Vue component, a standalone vue app or a vuetify app (wrapping with v-app)"
                    })
                )
                .WithBooleanField("Disabled", "Disable the form", "1",
                    new BooleanFieldSettings()
                    {
                        Hint = "If disabled, the Disabled Html field will be rendered in place of the form "
                    }
                )
                .WithBooleanField("Debug", "Debug", "2",
                    new BooleanFieldSettings()
                    {
                        Hint = "Debug mode is meant to be used when developping VueForms and will return additional debug information to the client along with the response"
                    }
                )
                .WithField("DisabledHtml", f => f
                    .OfType(nameof(HtmlField))
                    .WithDisplayName("Disabled Html")
                    .WithSettings(new HtmlFieldSettings() { Hint = "Html displayed when someone tries to render a disabled form.", SanitizeHtml = true })
                    .WithPosition("2")
                )
                .WithField("Template", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Template")
                   .WithSettings(new TextFieldSettings() { Required = true, Hint = "The form's template. VeeValidate v3(https://logaretm.github.io/vee-validate/guide/basics.html) is used for client side validation support. With liquid support." })
                   .WithPosition("3")
                   .WithEditor("Monaco")
                   .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"html\"}"
                        })
                )
                .WithDescription("Turns your content items into a vue form."));

            // add alias to VueForm and remove FlowPart
            _contentDefinitionManager.AlterTypeDefinition("VueForm", type => type
                .RemovePart("FlowPart")
                .WithPart("AliasPart", p => p.WithPosition("2")));

            // switch the editors to monaco
            _contentDefinitionManager.AlterPartDefinition("VueFormScripts", part => part
                .WithField("ClientInit", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("Client Init")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs client side to set various options for your form (such as setup the VeeValidate locales). Liquid is evaluated before passing the script to the client." })
                    .WithPosition("0")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\", \"renderValidationDecorations\": \"off\"}"
                        })
                )
                .WithField("ComponentOptions", f => f
                   .OfType(nameof(TextField))
                   .WithDisplayName("Component Options object")
                   .WithSettings(new TextFieldSettings() { Hint = "The form's vue component options object (https://vuejs.org/v2/api/#Options-Data). The component's data object is sent to the server. Liquid is evaluated before passing the script to the client." })
                   .WithPosition("1")
                .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\", \"renderValidationDecorations\": \"off\"}"
                        })
                )
                .WithField("OnValidation", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Validation")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side to validate your form." })
                    .WithPosition("2")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\"}"
                        })
                )
                .WithField("OnSubmitted", f => f
                    .OfType(nameof(TextField))
                    .WithDisplayName("On Submitted")
                    .WithSettings(new TextFieldSettings() { Hint = "(Optional) Script that runs server side after form validation has passed, before the workflow event is triggerred." })
                    .WithPosition("3")
                    .WithEditor("Monaco")
                    .WithSettings(
                        new TextFieldMonacoEditorSettings()
                        {
                            Options = "{\"language\": \"javascript\"}"
                        })
                )
                .Attachable()
                .WithDescription("Script fields for VueForms"));

            // migrate existing data
            var forms = await _session.Query<ContentItem, ContentItemIndex>(c => c.ContentType == "VueForm").ListAsync();

            foreach (var form in forms)
            {
                // var onValidation = (string)form.Content.VueFormScripts.OnValidation?.Text;
                // if(!string.IsNullOrEmpty(onValidation))
                // {
                //     var onSubmitted = (string)form.Content?.VueFormScripts?.OnSubmitted?.Text;

                //     form.Content.VueFormScripts.OnSubmitted.Text = "function Validate() { \n //Moved from the OnValidation script \n" + onValidation + "\n} Validate();\n" + onSubmitted;
                // }
                // form.Content.VueFormScripts.OnValidation = null;

                var flow = form.As<FlowPart>();
                if (flow != null)
                {
                    var templateBuilder = new StringBuilder();
                    foreach (var widget in flow.Widgets)
                    {
                        if (widget.ContentType == "VueComponent")
                        {
                            templateBuilder.Append((string)widget.Content.VueComponent.Template.Text);
                            templateBuilder.AppendLine();
                        }
                    }
                    form.Remove("FlowPart");
                    form.Alter<VueForm>(v =>
                    {
                        v.Template = new TextField() { Text = templateBuilder.ToString() };
                    });
                }
                if(form.Content?.VueForm?.Disabled == null)
                {
                    var isDisabled = false;
                    if (form.Content?.VueForm?.Enabled?.Value != null)
                    {
                        isDisabled = !(bool)form.Content.VueForm.Enabled.Value;
                    }
                    form.Content.VueForm.Enabled = null;

                    form.Alter<VueForm>(v =>
                    {
                        v.Disabled = new BooleanField() { Value = isDisabled };
                    });
                }
                _session.Save(form);
            }

            return 7;
        }

        private void AddVTextField()
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
}
