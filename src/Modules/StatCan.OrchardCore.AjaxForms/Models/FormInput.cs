using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormInput: ContentPart
    {
        public TextField Name { get; set; }
        public TextField Type { get; set; }
        public TextField Label { get; set; }
        public TextField Placeholder { get; set; }
        public TextField HelpText { get; set; }
    }
}