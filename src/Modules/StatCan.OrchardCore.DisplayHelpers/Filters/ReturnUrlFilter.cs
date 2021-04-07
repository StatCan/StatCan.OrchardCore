using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class ReturnUrlFilter : ILiquidFilter
    {
        private readonly HttpContext _httpContext;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ReturnUrlFilter(IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _urlHelperFactory = urlHelperFactory;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(context.ViewContext);

            var returnPath = (_httpContext.Request.PathBase + _httpContext.Request.Path).ToString();
            if (arguments["type"].Or(arguments.At(0)).ToStringValue() != "")
            {
                returnPath = arguments["type"].Or(arguments.At(0)).ToStringValue();
            }
            returnPath = returnPath.TrimEnd('/').Trim();

            if (input.ToStringValue() == "")
            {
                return new ValueTask<FluidValue>(new StringValue(((IUrlHelper)urlHelper).Content(returnPath)));
            }

            var targetPath = input.ToStringValue().TrimEnd('/').Trim();
            return new ValueTask<FluidValue>(new StringValue(((IUrlHelper)urlHelper).Content(string.Concat(targetPath, "?returnUrl=", returnPath))));
        }
    }
}
