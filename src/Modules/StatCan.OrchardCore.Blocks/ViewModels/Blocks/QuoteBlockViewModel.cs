namespace Etch.OrchardCore.Blocks.ViewModels.Blocks
{
    public class QuoteBlockViewModel
    {
        public string Alignment { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }

        public bool HasCaption
        {
            get { return !string.IsNullOrWhiteSpace(Caption); }
        }
    }
}
