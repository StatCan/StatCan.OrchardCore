using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Markdown.Fields;
using OrchardCore.Taxonomies.Fields;

namespace StatCan.OrchardCore.Scheduling.Models
{
    public class Appointment : ContentPart
    {
        public DateTimeField StartDate { get; set; }
        public DateTimeField EndDate { get; set; }
        public TaxonomyField Calendar { get; set; }
        public ContentPickerField LinkedContent { get; set; }
        public TextField Color { get; set; }
        public TextField Status { get; set; }
        public MarkdownField Comments { get; set; }
    }
}
