using Newtonsoft.Json;
using OrchardCore.ContentFields.Settings;

namespace StatCan.OrchardCore.ContentFields.MultiValueTextField.Settings
{
    public class MultiValueTextFieldEditorSettings
    {
        public ListValueOption[] Options { get; set; }
        public EditorOption Editor { get; set; }
        public string DefaultValue { get; set; }
    }

    public enum EditorOption
    {
        Checkbox,
        Dropdown,
        Picker
    }
}
