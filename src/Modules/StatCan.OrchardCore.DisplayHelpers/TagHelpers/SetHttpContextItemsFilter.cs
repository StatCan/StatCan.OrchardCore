using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OrchardCore.Mvc.Utilities;

namespace StatCan.OrchardCore.DisplayHelpers.TagHelpers
{
    /// <summary>
    /// Use this helper to set HttpContext.Items values via attributes
    /// Usage: {% helper "set-context-items", inno-form-data: formData %}
    /// inno-form-data will become InnoFormData in the context
    /// </summary>
    [HtmlTargetElement("set-context-items", TagStructure = TagStructure.WithoutEndTag)]
  public class SetHttpContextItemsTagHelper : TagHelper
    {
     private readonly IHttpContextAccessor _httpContextAccessor;

      public SetHttpContextItemsTagHelper(IHttpContextAccessor httpContextAccessor)
      {
        _httpContextAccessor = httpContextAccessor;
      }

      public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
      {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext != null)
        {
          foreach (var item in context.AllAttributes)
          {
            httpContext.Items.Add(item.Name.ToPascalCaseDash(), item.Value);
          }
        }

        return Task.CompletedTask;
      }
    }
}