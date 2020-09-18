using System.Threading.Tasks;
using Lombiq.HelpfulExtensions.Extensions.Flows.Models;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;

namespace Lombiq.HelpfulExtensions.Extensions.Flows.Handlers
{
    public class AdditionalStylingPartHandler : ContentHandlerBase
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;


        public AdditionalStylingPartHandler(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }


        public override Task ActivatedAsync(ActivatedContentContext context)
        {
            if (!context.ContentItem.Has<AdditionalStylingPart>() &&
                _contentDefinitionManager.GetTypeDefinition(context.ContentItem.ContentType)
                    .GetSettings<ContentTypeSettings>().Stereotype == "Widget")
            {
                context.ContentItem.Weld<AdditionalStylingPart>();
            }

            return Task.CompletedTask;
        }
    }
}
