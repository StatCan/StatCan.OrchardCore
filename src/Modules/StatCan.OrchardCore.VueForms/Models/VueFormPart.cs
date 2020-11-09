using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueForm : ContentPart
    {
        public BooleanField Enabled { get; set; }
        public TextField RenderAs { get; set; }
        public TextField SuccessMessage { get; set; }
        public HtmlField DisabledHtml { get; set; }
    }
}
