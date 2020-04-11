using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormTextInput : ContentPart
    {
        public TextField Name { get; set; }
        public TextField Type { get; set; }

        public TextField Label { get; set; }
        public TextField Placeholder { get; set; }
        public TextField HelpText { get; set; }

        // validation
        public TextField ValidText { get; set; }
        public TextField InvalidText { get; set; }
        public BooleanField Required { get; set; }
        // public NumericField MinLength { get; set; }
        // public NumericField MaxLength { get; set; }
        // public TextField RegularExpression { get; set; }

        // styles
        public TextField WrapperClass { get; set; }
        public TextField LabelClass { get; set; }
        public TextField InputClass { get; set; }
    }
}