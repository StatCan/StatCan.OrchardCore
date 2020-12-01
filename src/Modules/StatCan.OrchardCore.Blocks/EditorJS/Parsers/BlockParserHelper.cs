namespace Etch.OrchardCore.Blocks.EditorJS.Parsers
{
    public class BlockParserHelper
    {
        public static string AddPathBaseToRelativeLinks(string requestPathBase, string content)
        {
            if (string.IsNullOrWhiteSpace(requestPathBase))
            {
                return content;
            }

            return content.Replace("<a href=\"/", $"<a href=\"{requestPathBase}/");
        }
    }
}
