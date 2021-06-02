using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueForm : ContentPart
    {
        public TextField Template { get; set; }
        public TextField RenderAs { get; set; }
        public BooleanField Disabled { get; set; }
        public HtmlField DisabledHtml { get; set; }
    }
}
