using Newtonsoft.Json;

namespace StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings
{
    public class TextFieldPredefinedGroupEditorSettings
    {
        public ListValueOption[] Options { get; set; }
        public EditorOption Editor { get; set; }
        public string DefaultValue { get; set; }
    }

    public enum EditorOption
    {
        ButtonGroup
    }

    public class ListValueOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
