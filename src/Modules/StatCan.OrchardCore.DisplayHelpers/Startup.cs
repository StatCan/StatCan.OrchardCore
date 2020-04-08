using Ganss.XSS;
using StatCan.OrchardCore.DisplayHelpers.Filters;
using StatCan.OrchardCore.DisplayHelpers.EventHandlers;
using StatCan.OrchardCore.DisplayHelpers.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Liquid;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.DisplayHelpers
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHtmlSanitizer>(_ => {
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("class");
                sanitizer.AllowedAttributes.Add("id");
                sanitizer.AllowedAttributes.Add("role");
                sanitizer.AllowDataAttributes = true;
                sanitizer.AllowedSchemes.Add("mailto");
                return sanitizer;
                });
            services.AddLiquidFilter<SanitizeHtmlFilter>("sanitize_html");
            services.AddLiquidFilter<BoolFilter>("bool");
            services.AddLiquidFilter<ClonePropertiesFilter>("clone_properties");
            services.AddLiquidFilter<UserEmailFilter>("user_email");
            services.AddLiquidFilter<IsCurrentUrlFilter>("is_current_url");
            services.AddTagHelpers<SetHttpContextItemsTagHelper>();
            services.AddScoped<ILiquidTemplateEventHandler, HttpContextItemsEventHandler>();
        }
    }
}