using Newtonsoft.Json;

namespace StatCan.OrchardCore.ContentFields.Multivalue.Settings
{
    public class MultivalueFieldEditorSettings
    {
        public ListValueOption[] Options { get; set; }
        public string DefaultValue { get; set; }
    }

    public class ListValueOption
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
