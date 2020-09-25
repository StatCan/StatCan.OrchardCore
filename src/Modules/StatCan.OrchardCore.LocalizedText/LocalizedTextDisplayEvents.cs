using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.Implementation;
using StatCan.OrchardCore.LocalizedText.Fields;

namespace StatCan.OrchardCore.LocalizedText
{
    /// <summary>
    /// This class will set the closest ContentItem that has a LocalizedTextPart to allow
    /// child widgets in a flow / bag to access the LocalizedText of the previously rendered item.
    /// Note: This will not work if a parent and widget both implement the LocalizedText part.
    /// The "parent" resolution will not work if a widget that is rendered after another that has this part.
    /// </summary>
    public class LocalizedTextDisplayEvents : IShapeDisplayEvents, ILocalizedTextAccessor
    {
        public ContentItem Item { get; private set; }

        public Task DisplayedAsync(ShapeDisplayContext context)
        {
            return Task.CompletedTask;
        }

        public Task DisplayingAsync(ShapeDisplayContext context)
        {
            var shape = context.Shape;
            if(shape.Properties.TryGetValue("ContentItem", out var item))
            {
                var ltp = (item as ContentItem)?.As<LocalizedTextPart>();
                if (ltp != null)
                {
                    Item = (ContentItem)item;
                }
            }
            return Task.CompletedTask;
        }

        public Task DisplayingFinalizedAsync(ShapeDisplayContext context)
        {
            return Task.CompletedTask;
        }
    }

    public interface ILocalizedTextAccessor
    {
          ContentItem Item { get;}

    }
}