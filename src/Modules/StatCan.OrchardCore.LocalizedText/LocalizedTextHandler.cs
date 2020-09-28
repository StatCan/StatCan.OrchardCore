using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using StatCan.OrchardCore.LocalizedText.Fields;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.LocalizedText
{
    //This supports the case where the Item is loaded by the content manager and then some liquid script is executed.
    public class LocalizedTextHandler : ContentHandlerBase
    {
        private readonly ILocalizedTextAccessor _accessor;

        public LocalizedTextHandler(ILocalizedTextAccessor accessor)
        {
            _accessor = accessor;
        }

        public override Task LoadedAsync(LoadContentContext context)
        {
            var part = context.ContentItem.As<LocalizedTextPart>();
            if (part != null)
            {
                _accessor.AddLocalizedItem(part);
            }
            return base.LoadedAsync(context);
        }
    }
}