namespace StatCan.OrchardCore.CommonTypes
{
    public static class FeatureIds
    {
        private const string FeatureIdPrefix = "StatCan.OrchardCore.CommonTypes.";

        public const string Page = FeatureIdPrefix + nameof(Page);
        public const string AdditionalPages = FeatureIdPrefix + nameof(AdditionalPages);
        public const string SecurePage = FeatureIdPrefix + nameof(SecurePage);
        public const string HtmlWidget = FeatureIdPrefix + "HtmlWidget";
        public const string LiquidWidget = FeatureIdPrefix + "LiquidWidget";
        public const string MarkdownWidget = FeatureIdPrefix + "MakrdownWidget";
    }
}
