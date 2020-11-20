using StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings;

namespace StatCan.OrchardCore.ContentFields.PredefinedGroup.ViewModels
{
    public class PredefinedGroupSettingsViewModel
    {
        public PredefinedGroupEditorOptions Editor { get; set; }
        public string Options { get; set; }
        public string DefaultValue { get; set; }
    }
}
