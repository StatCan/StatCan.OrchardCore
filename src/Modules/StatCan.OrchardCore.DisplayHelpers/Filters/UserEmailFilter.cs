using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Identity;
using OrchardCore.Liquid;
using OrchardCore.Users;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class UserEmailFilter : ILiquidFilter
    {
        private readonly UserManager<IUser> _userManager;

        public UserEmailFilter(UserManager<IUser> userManager){
            _userManager = userManager;
        }

        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments args, TemplateContext context)
        {
            if(input.ToObjectValue() is IUser user) {
                return FluidValue.Create(await _userManager.GetEmailAsync(user));
            }
            return NilValue.Instance;
        }
    }
}