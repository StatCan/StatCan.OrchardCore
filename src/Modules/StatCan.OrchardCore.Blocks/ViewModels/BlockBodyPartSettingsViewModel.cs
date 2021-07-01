using StatCan.OrchardCore.Blocks.Settings;

namespace StatCan.OrchardCore.Blocks.ViewModels
{
    public class BlockBodyPartSettingsViewModel
    {
        public string[] LinkableContentTypes { get; set; } = new string[0];
        public BlockBodyPartSettings BlockBodyPartSettings { get; set; }
    }
}
