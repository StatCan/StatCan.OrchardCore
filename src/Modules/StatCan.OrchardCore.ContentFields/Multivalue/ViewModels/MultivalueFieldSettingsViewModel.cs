using StatCan.OrchardCore.ContentFields.Multivalue.Settings;

namespace StatCan.OrchardCore.ContentFields.Multivalue.ViewModels
{
    public class MultivalueFieldSettingsViewModel
    {
        public EditorOption Editor { get; set; }
        public string Options { get; set; }
        public string DefaultValue { get; set; }
    }
}
