using StatCan.OrchardCore.DisplayHelpers.Filters;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.DisplayHelpers
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLiquidFilter<BoolFilter>("bool");
            services.AddLiquidFilter<ClonePropertiesFilter>("clone_properties");
            services.AddLiquidFilter<IsCurrentUrlFilter>("is_current_url");
            services.AddLiquidFilter<GetClaimLiquidFilter>("get_claim");
            services.AddLiquidFilter<ReturnUrlFilter>("return_url");
            services.AddLiquidFilter<UsersByRoleFilter>("users_by_role");
            services.AddLiquidFilter<Base64Filter>("base64_encode");
        }
    }
}
