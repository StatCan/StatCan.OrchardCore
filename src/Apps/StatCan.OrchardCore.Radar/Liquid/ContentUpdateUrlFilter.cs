using System.Linq;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ContentUpdateUrlFilter : ILiquidFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContentUpdateUrlFilter(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
        {
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var item = input.ToObjectValue() as ContentItem;

            if (item == null)
            {
                throw new ArgumentException("ContentItem missing while calling 'content_update_url' filter");
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(context.ViewContext);
            string url = "";

            if (item.ContentType == "Project")
            {
                url = $"~/projects/update/{item.ContentItemId}";
            }
            else if (item.ContentType == "Proposal")
            {
                url = $"~/proposals/update/{item.ContentItemId}";
            }
            else if (item.ContentType == "Community")
            {
                url = $"~/communities/update/{item.ContentItemId}";
            }
            else if (item.ContentType == "Event")
            {
                url = $"~/events/update/{item.ContentItemId}";
            }
            else if (item.ContentType == "Topic")
            {
                url = $"~/topics/update/{item.ContentItemId}";
            }
            else if (item.ContentType == "Artifact")
            {
                url = GetArtifactPath(_httpContextAccessor.HttpContext.Request.Path, item);
            }

            if(!(bool)arguments["relative"].ToObjectValue())
            {
                return new ValueTask<FluidValue>(new StringValue((urlHelper).Content(url)));
            }

            return FluidValue.Create(url, context.Options);
        }

        // The logic here is purely based on the structure of the url. 
        private string GetArtifactPath(string path, ContentItem artifact)
        {
            string[] pathValues = path.Substring(1).Split("/");

            if (pathValues.Length == 2)
            {
                return $"{pathValues[pathValues.Length - 1]}/artifacts/update/{artifact.ContentItemId}";
            }
            else if(pathValues.Length == 4)
            {
                return $"update/{artifact.ContentItemId}";
            }

            return "";
        }
    }
}
