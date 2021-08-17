
using StatCan.OrchardCore.Vuetify.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Vuetify.Handlers
{
    public class WidgetStylingPartHandler : ContentHandlerBase
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public WidgetStylingPartHandler(IContentDefinitionManager contentDefinitionManager) =>
            _contentDefinitionManager = contentDefinitionManager;
        public override Task ActivatedAsync(ActivatedContentContext context)
        {
            if (!context.ContentItem.Has<WidgetStylingPart>() &&
                _contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType)
                    .GetSettings<ContentTypeSettings>().Stereotype == "Widget")
            {
                context.ContentItem.Weld<WidgetStylingPart>();
            }

            return Task.CompletedTask;
        }
    }
}
