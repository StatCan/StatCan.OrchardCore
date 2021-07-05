using System;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.Liquid;
using Shortcodes;
using Cysharp.Text;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    // Converts shortcode arguments to html arguments
    public class ShortcodeArgsToString : ILiquidFilter
    {
        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext ctx)
        {
            var inputObj = input.ToObjectValue();

            if(inputObj is Arguments shortcodeArgs)
            {
                using var sb = ZString.CreateStringBuilder();
                sb.Append(" ");
                var whitelist = arguments.At(0)?.ToStringValue()?.Split(',');

                foreach(var arg in shortcodeArgs)
                {
                    if(whitelist?.Contains(arg.Key) == true)
                    {
                        sb.Append($"{arg.Key}=\"{arg.Value}\" ");
                    }
                }
                return new StringValue(sb.ToString());
            }
            return StringValue.Empty;
        }
    }
}
