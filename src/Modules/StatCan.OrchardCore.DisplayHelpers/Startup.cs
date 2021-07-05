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
            services.AddLiquidFilter<SectionIsNotEmpty>("section_not_empy");
            services.AddLiquidFilter<B64EncodeFilter>("base64_encode");
            services.AddLiquidFilter<B64DecodeFilter>("base64_decode");
        }
    }

    [RequireFeatures("OrchardCore.Taxonomies")]
     public class TaxonomiesStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLiquidFilter<AllTaxonomyTermsFilter>("all_taxonomy_terms");
        }
    }
    [RequireFeatures("OrchardCore.Shortcodes")]
     public class ShortcodesStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddLiquidFilter<ShortcodeArgsToString>("to_html_attributes");
        }
    }
}
