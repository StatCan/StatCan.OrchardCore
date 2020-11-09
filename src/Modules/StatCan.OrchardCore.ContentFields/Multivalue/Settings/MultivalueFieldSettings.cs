namespace StatCan.OrchardCore.ContentFields.Multivalue.Settings
{
    public class MultivalueFieldSettings
    {
        public string Hint { get; set; }
        public bool Required { get; set; }
        public bool DisplayAllContentTypes { get; set; }
        public string[] DisplayedContentTypes { get; set; } = new string[0];
    }
}
