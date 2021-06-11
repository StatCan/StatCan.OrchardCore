using System;
using System.Linq;
using System.Threading.Tasks;
using Fluid;
using Fluid.Values;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;
using OrchardCore.Security;
using OrchardCore.Users.Indexes;
using OrchardCore.Users.Models;
using YesSql;
using YesSql.Services;

namespace StatCan.OrchardCore.DisplayHelpers.Filters
{
    public class UsersByRoleFilter : ILiquidFilter
    {
        public async ValueTask<FluidValue> ProcessAsync(FluidValue input, FilterArguments arguments, LiquidTemplateContext ctx)
        {
            var session = ctx.Services.GetRequiredService<ISession>();
            var roleManager = ctx.Services.GetRequiredService<RoleManager<IRole>>();

            if (input.Type == FluidValues.Array)
            {
                // List of roles passed
                var userRoles = input.Enumerate().Select(x => roleManager.NormalizeKey(x.ToStringValue())).ToArray();
                 var query = session.Query<User>();
                 query.With<UserByRoleNameIndex>(x => x.RoleName.IsIn(userRoles));

                return FluidValue.Create(await query.ListAsync(), ctx.Options);
            }
            else
            {
                var normalizedRoleName = roleManager.NormalizeKey(input.ToStringValue());
                return  FluidValue.Create(await session.Query<User, UserByRoleNameIndex>(u => u.RoleName == normalizedRoleName).ListAsync(), ctx.Options);
            }
        }
    }
}
