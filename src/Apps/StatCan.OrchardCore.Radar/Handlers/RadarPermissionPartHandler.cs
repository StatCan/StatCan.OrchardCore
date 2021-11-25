using System.Threading.Tasks;
using OrchardCore.ContentManagement.Handlers;
using StatCan.OrchardCore.Radar.Models;

namespace StatCan.OrchardCore.Radar.Handlers
{
    public class RadarPermissionPartHandler : ContentPartHandler<RadarPermissionPart>
    {
        public override Task UpdatedAsync(UpdateContentContext context, RadarPermissionPart instance)
        {
            context.ContentItem.Content.RadarPermissionPart.ContentItemId = context.ContentItem.ContentItemId;
            context.ContentItem.Content.RadarPermissionPart.ContentItemType = context.ContentItem.ContentType;
            context.ContentItem.Content.RadarPermissionPart.Owner = context.ContentItem.Owner;

            return Task.CompletedTask;
        }
    }
}
