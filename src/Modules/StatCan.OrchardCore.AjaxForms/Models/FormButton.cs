using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormButton : ContentPart
    {
        public TextField Name { get; set; }
        public TextField Type { get; set; }
        public TextField Label { get; set; }
        public BooleanField DisableOnSubmit { get; set; }
        public TextField CssClass { get; set; }
    }
}