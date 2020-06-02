using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace StatCan.OrchardCore.Security
{
    public static class SecurityExtensions
    {
        public static IApplicationBuilder UseStatCanSecurityHeaders(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
            var policyCollection = new HeaderPolicyCollection()
            .AddFrameOptionsSameOrigin()
            .AddXssProtectionEnabled()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader()
            .AddContentSecurityPolicy(builder =>
            {
                builder.AddObjectSrc().Self();
                // Domains here for OAuth redirects
                builder.AddFormAction().Self().From("github.com").From("account.gccollab.ca").From("login.microsoftonline.com");
                builder.AddFrameAncestors().Self();
                builder.AddDefaultSrc().Self();
                builder.AddImgSrc().Self().Data().From("*.statcan.ca").From("*.statcan.gc.ca");
                builder.AddFontSrc().Self().Data().From("cdn.jsdelivr.net").From("fonts.googleapis.com").From("fonts.gstatic.com");
                builder.AddStyleSrc().UnsafeInline().Self()
                .From("cdn.jsdelivr.net")
                .From("fonts.googleapis.com")
                .From("code.jquery.com")
                .From("unpkg.com")
                .From("cdnjs.cloudflare.com")
                .From("stackpath.bootstrapcdn.com")
                ;
                // unsafe-eval needed for vue.js runtime templates
                builder.AddScriptSrc().UnsafeEval().UnsafeInline().Self()
               .From("cdn.jsdelivr.net")
               .From("code.jquery.com")
               .From("ajax.googleapis.com")
               .From("cdnjs.cloudflare.com")
               .From("vuejs.org")
               .From("unpkg.com")
               .From("stackpath.bootstrapcdn.com")
               .From("*.statcan.ca")
               .From("*.statcan.gc.ca")
               ;
            });

            if (!env.IsDevelopment())
            {
                policyCollection.AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 60 * 60 * 24 * 365); // maxage = one year in seconds
            }

            return app.UseSecurityHeaders(policyCollection);
        }

        public static IApplicationBuilder UseStatCanCookiePolicy(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IHostEnvironment>();
            var cookiePolicy = new CookiePolicyOptions()
            {
                HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            };
            if (env.IsDevelopment())
            {
                cookiePolicy.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
            }
            else
            {
                cookiePolicy.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
            }
            return app.UseCookiePolicy(cookiePolicy);
        }
    }
}
