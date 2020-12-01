namespace Etch.OrchardCore.Blocks.ViewModels.Blocks
{
    public class EmbedBlockViewModel
    {
        public string Caption { get; set; }
        public string Service { get; set; }
        public string SourceUrl { get; set; }

        public bool HasCaption
        {
            get { return !string.IsNullOrWhiteSpace(Caption); }
        }
    }
}
