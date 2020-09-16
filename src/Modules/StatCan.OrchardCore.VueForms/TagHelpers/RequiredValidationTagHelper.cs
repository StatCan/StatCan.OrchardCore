using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using StatCan.OrchardCore.VueForms.Models;

namespace StatCan.OrchardCore.VueForms
{
    //[HtmlTargetElement("input", Attributes = AttributeName, TagStructure = TagStructure.WithoutEndTag)]
    //public class RequiredValidationTagHelper: TagHelper
    //{
    //    private const string AttributeName = "form-required-val";

    //    private readonly IStringLocalizer S;

    //    [HtmlAttributeName(AttributeName)]
    //    public FormRequiredValidation RequiredValidation { get; set; }

    //    public RequiredValidationTagHelper(IStringLocalizer<RequiredValidationTagHelper> localizer)
    //    {
    //        S = localizer;
    //    }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        if(RequiredValidation?.Required?.Value == true)
    //        {
    //            output.Attributes.Add(new TagHelperAttribute("data-val", "true"));
    //            output.Attributes.Add(new TagHelperAttribute("data-val-required",  RequiredValidation.RequiredText?.Text ?? S["This field is required"]));
    //        }

    //    }
    //}
}
