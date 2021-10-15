using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using Etch.OrchardCore.ContentPermissions.Services;
using System.Collections;
using System.Collections.Generic;

namespace StatCan.OrchardCore.Radar.Liquid
{
    public class ListCanViewFilter: ILiquidFilter
    {
        private readonly IContentPermissionsService _contentPermissionsService;

        public ListCanViewFilter(IContentPermissionsService contentPermissionsService)
        {
            _contentPermissionsService = contentPermissionsService;
        }

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext context)
        {
            var items = input.ToObjectValue() as IEnumerable;
            var contentItems = new List<ContentItem>();

            foreach(var item in items)
            {
                var contentItem = item as ContentItem;

                if(_contentPermissionsService.CanAccess(contentItem))
                {
                    contentItems.Add(contentItem);
                }
            }

            return FluidValue.Create(contentItems, context.Options);
        }
    }
}
