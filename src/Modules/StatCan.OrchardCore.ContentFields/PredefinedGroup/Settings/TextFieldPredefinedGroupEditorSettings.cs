using Newtonsoft.Json;
using OrchardCore.ContentFields.Settings;

namespace StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings
{
    public class TextFieldPredefinedGroupEditorSettings
    {
        public ListValueOption[] Options { get; set; }
        public PredefinedGroupEditorOptions Editor { get; set; }
        public string DefaultValue { get; set; }
    }

    public enum PredefinedGroupEditorOptions
    {
        ButtonGroup
    }

}
