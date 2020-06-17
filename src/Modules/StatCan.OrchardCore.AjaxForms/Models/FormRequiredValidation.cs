using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormRequiredValidation : ContentPart
    {
        // validation
        public BooleanField Required { get; set; }
        public TextField RequiredText { get; set; }
    }
    public class FormInputValidation: ContentPart
    {
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