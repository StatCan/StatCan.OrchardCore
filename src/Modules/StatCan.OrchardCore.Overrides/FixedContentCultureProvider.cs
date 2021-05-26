using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentLocalization;
using OrchardCore.ContentLocalization.Services;

namespace StatCan.OrchardCore.Overrides
{
    /// <summary>
    /// RequestCultureProvider that automatically sets the Culture of a request from the LocalizationPart.Culture property.
    /// This one overrides the default Orchard RequestCultureProvider to also set the cookie to avoid having out of sync locales
    /// </summary>
    public class FixedContentCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }


            var culturePickerService = httpContext.RequestServices.GetService<IContentCulturePickerService>();
            if(culturePickerService == null)
            {
                return null;
            }

            var options = httpContext.RequestServices.GetRequiredService<IOptions<CulturePickerOptions>>();
            var localization = await culturePickerService.GetLocalizationFromRouteAsync(httpContext.Request.Path);

            if (localization != null)
            {
                    httpContext.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
                    httpContext.Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(localization.Culture)),
                        new CookieOptions { Expires = DateTime.UtcNow.AddDays(options.Value.CookieLifeTime) }
                    );
                return new ProviderCultureResult(localization.Culture);
            }

            return null;
        }
    }
}
