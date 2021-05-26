using System;
using System.Linq;
using Fluid;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentLocalization;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Overrides
{
    [RequireFeatures("OrchardCore.ContentLocalization.ContentCulturePicker")]
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // this removes the ContentCultureProvider added by the ContentCulturePicker module
                var existing = options.RequestCultureProviders.FirstOrDefault(t=> t.GetType() == typeof(ContentRequestCultureProvider));
                if (existing != null)
                {
                    options.RequestCultureProviders.Remove(existing);
                }
                options.AddInitialRequestCultureProvider(new FixedContentCultureProvider());
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}
