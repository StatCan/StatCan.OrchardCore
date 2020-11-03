using Etch.OrchardCore.ContentPermissions.Services;
using Fluid;
using Fluid.Values;
using OrchardCore.ContentManagement;
using OrchardCore.Liquid;
using System;
using System.Threading.Tasks;

namespace Etch.OrchardCore.ContentPermissions.Liquid
{
    public class UserCanViewFilter : ILiquidFilter
    {
        #region Dependencies

        private readonly IContentPermissionsService _contentPermissionsService;

        #endregion

        #region Constructor

        public UserCanViewFilter(IContentPermissionsService contentPermissionsService)
        {
            _contentPermissionsService = contentPermissionsService;
        }

        #endregion

        #region Implementation

        public ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, TemplateContext context)
        {
            var item = input.ToObjectValue() as ContentItem;

            if (item == null)
            {
                throw new ArgumentException("ContentItem missing while calling 'user_can_view' filter");
            }

            if (_contentPermissionsService.CanAccess(item))
            {
                return new ValueTask<FluidValue>(BooleanValue.True);
            }

            return new ValueTask<FluidValue>(BooleanValue.False);
        }

        #endregion
    }
}
