using Etch.OrchardCore.Blocks.Settings;

namespace Etch.OrchardCore.Blocks.ViewModels
{
    public class BlockBodyPartSettingsViewModel
    {
        public string[] LinkableContentTypes { get; set; } = new string[0];
        public BlockBodyPartSettings BlockBodyPartSettings { get; set; }
    }
}
