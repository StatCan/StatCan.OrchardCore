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
            var whitelist = arguments.At(0)?.ToStringValue()?.Split(',');

            if(inputObj is Arguments shortcodeArgs && whitelist?.Length > 0)
            {
                using var sb = ZString.CreateStringBuilder();
                sb.Append(" ");

                foreach(var arg in shortcodeArgs)
                {
                    // todo: remove this when OC supports - in shortcode arguments
                    // temporary code to support arguments that contain a - since OC currently does not support
                    var key = arg.Key.Replace('_', '-');
                    if(whitelist.Contains(key))
                    {
                        sb.Append($"{key}=\"{arg.Value}\" ");
                    }
                    else if (whitelist.Contains(arg.Value))
                    {
                        // for boolean arguments that are valid for vue.js
                        sb.Append($"{arg.Value} ");
                    }
                }
                return new StringValue(sb.ToString());
            }
            return StringValue.Empty;
        }
    }
}
