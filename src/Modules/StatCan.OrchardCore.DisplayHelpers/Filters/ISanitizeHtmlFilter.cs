using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Ganss.XSS;
using OrchardCore.Liquid;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class SanitizeHtmlFilter : ILiquidFilter
    {
        private readonly IHtmlSanitizer _sanitizer;

        public SanitizeHtmlFilter(IHtmlSanitizer sanitizer)
        {
            _sanitizer = sanitizer;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext ctx)
        {
            return new ValueTask<FluidValue>(new StringValue(_sanitizer.Sanitize(input.ToStringValue())));
        }
    }
}
