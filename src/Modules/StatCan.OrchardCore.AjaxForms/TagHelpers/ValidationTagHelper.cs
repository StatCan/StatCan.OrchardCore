using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using StatCan.OrchardCore.AjaxForms.Models;

namespace StatCan.OrchardCore.AjaxForms
{
    
    [HtmlTargetElement("input", Attributes = AttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class ValidationTagHelper: TagHelper
    {
        private const string AttributeName = "form-input-validation";

        private readonly IStringLocalizer<ValidationTagHelper> _localizer;

        [HtmlAttributeName(AttributeName)]
        public FormInput FormInput { get; set; }

        public ValidationTagHelper(IStringLocalizer<ValidationTagHelper> localizer)
        {
            this._localizer = localizer;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(FormInput.Required.Value)
            {
                output.Attributes.Add(new TagHelperAttribute("data-val", "true"));
                output.Attributes.Add(new TagHelperAttribute("data-val-required", "Some validation text"));
            }

        }
    }
}
