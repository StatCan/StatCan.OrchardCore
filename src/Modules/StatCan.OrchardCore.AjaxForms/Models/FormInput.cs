using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormInput : ContentPart
    {
        public TextField Name { get; set; }
        public TextField Type { get; set; }

        public TextField Label { get; set; }
        public TextField Placeholder { get; set; }
        public TextField HelpText { get; set; }

        // styles
        public TextField WrapperClass { get; set; }
        public TextField LabelClass { get; set; }
        public TextField InputClass { get; set; }

        // validation
        public BooleanField Required { get; set; }
        public TextField RequiredText { get; set; }
        //public NumericField MinLength { get; set; }
        //public TextField MinLengthText { get; set; }
        //public NumericField MaxLength { get; set; }
        //public TextField MaxLengthText { get; set; }
        //public NumericField Min { get; set; }
        //public TextField MinText { get; set; }
        //public NumericField Max { get; set; }
        //public TextField MaxText { get; set; }
        //public TextField Pattern { get; set; }
        //public TextField PatternText { get; set; }
    }
}