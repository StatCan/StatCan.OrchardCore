using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Flows.Models;
using StatCan.OrchardCore.Radar.Models;

namespace StatCan.OrchardCore.Radar.Handlers
{
    public class RadarPermissionPartHandler : ContentPartHandler<RadarPermissionPart>
    {
        public override Task UpdatedAsync(UpdateContentContext context, RadarPermissionPart instance)
        {
            context.ContentItem.Content.RadarPermissionPart.ContentItemId = context.ContentItem.ContentItemId;
            context.ContentItem.Content.RadarPermissionPart.ContentType = context.ContentItem.ContentType;
            context.ContentItem.Content.RadarPermissionPart.Owner = context.ContentItem.Owner;
            context.ContentItem.Content.RadarPermissionPart.Published = context.ContentItem.ContentType == "Artifact" ? context.ContentItem.Published : context.ContentItem.Content.RadarEntityPart.Publish.Value;

            var workspace = context.ContentItem.Get<BagPart>("Workspace");

            if (workspace != null)
            {
                foreach (var artifact in workspace.ContentItems)
                {
                    artifact.ContentItem.Content.RadarPermissionPart.ParentContentItemId = context.ContentItem.ContentItemId;
                }

                context.ContentItem.Apply("Workspace", workspace);
            }

            return Task.CompletedTask;
        }
    }
}
