using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Taxonomies.Fields;

namespace StatCan.OrchardCore.Radar.Models
{
    public class RadarEntityPart : ContentPart
    {
        public TextField Name { get; set; }

        public TextField Description { get; set; }
        public TaxonomyField Topics { get; set; }

        public ContentPickerField RelatedEntity { get; set; }
    }
}
