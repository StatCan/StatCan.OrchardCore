using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.LocalizedText
{
    public class ContentItemAccessor : ContentHandlerBase, IContentItemAccessor
    {
        public ContentItem Item { get; private set; }

        public override Task LoadedAsync(LoadContentContext context)
        {
            if (Item == null)
            {
                Item = context.ContentItem;
            }
            return base.LoadedAsync(context);
        }
    }

    public interface IContentItemAccessor
    {
        ContentItem Item { get; }
    }
}
