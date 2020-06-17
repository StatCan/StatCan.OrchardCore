using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormInputStyle: ContentPart
    {
        public TextField WrapperClass { get; set; }
        public TextField LabelClass { get; set; }
        public TextField InputClass { get; set; }
    }
}