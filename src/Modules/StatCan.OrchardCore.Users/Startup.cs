using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;


namespace StatCan.OrchardCore.Users
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.OnAppendCookie = cookieContext =>
                {
                    // Disabling same-site is required for external authentication provider's support to work correctly.
                    if (cookieContext.CookieName.StartsWith("orchauth_"))
                    {
                        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
                    }
                };
            });
        }
    }
}