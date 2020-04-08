using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Fluid;
using Ganss.XSS;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore;
using OrchardCore.Liquid;

public static class LiquidRazorHelperExtensions
{
    public static string B64Encode(this IOrchardHelper orchardHelper, string toEncode)
    {
        return string.IsNullOrEmpty(toEncode) ? "" : System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(toEncode));
    }

    /// <summary>
    /// Parses a liquid string and returns the result as a string
    /// </summary>
    /// <param name="liquid">The liquid to parse.</param>
    /// <param name="model">A model to bind against.</param>
    public static Task<string> LiquidAsync(this IOrchardHelper orchardHelper, string liquid, object model = null)
    {
        var liquidTemplateManager = orchardHelper.HttpContext.RequestServices.GetRequiredService<ILiquidTemplateManager>();
        var htmlEncoder = orchardHelper.HttpContext.RequestServices.GetRequiredService<HtmlEncoder>();

        return liquidTemplateManager.RenderAsync(liquid, htmlEncoder, model);
    }

    /// <summary>
    /// Sanitizes html against XSS
    /// </summary>
    public static IHtmlContent SanitizedRawHtml(this IOrchardHelper orchardHelper, string html)
    {
        var htmlSanitizer = orchardHelper.HttpContext.RequestServices.GetRequiredService<IHtmlSanitizer>();

        return new HtmlString(htmlSanitizer.Sanitize(html));
    }

    /// <summary>
    /// Parses a liquid string to HTML
    /// </summary>
    /// <param name="liquid">The liquid to parse.</param>
    /// <param name="model">(optional)A model to bind against.</param>
    public static async Task<IHtmlContent> LiquidToSanitizedHtmlAsync(this IOrchardHelper orchardHelper, string liquid, object model = null)
    {
        var liquidTemplateManager = orchardHelper.HttpContext.RequestServices.GetRequiredService<ILiquidTemplateManager>();
        var htmlSanitizer= orchardHelper.HttpContext.RequestServices.GetRequiredService<IHtmlSanitizer>();
        var htmlEncoder = orchardHelper.HttpContext.RequestServices.GetRequiredService<HtmlEncoder>();

        liquid = await liquidTemplateManager.RenderAsync(liquid, htmlEncoder, model);

        return new HtmlString(htmlSanitizer.Sanitize(liquid));
    }
}

