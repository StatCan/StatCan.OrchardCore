using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.Radar.Models
{
    public class RadarPermissionPart : ContentPart
    {
        public string ContentItemId { get; set; }
        public string ContentType { get; set; }
        public string Owner { get; set; }
        public bool Published { get; set; }
    }
}
