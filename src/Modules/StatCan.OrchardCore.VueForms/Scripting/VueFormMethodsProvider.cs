using System;
using System.Collections.Generic;
using AngleSharp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.VueForms.Scripting
{
    /// <summary>
    /// This does not need to be registered in the DI as these methods are only used in the VueFormController
    /// </summary>
    public class VueFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _getSurveyData;
        private readonly GlobalMethod _getFormContentItem;
        private readonly GlobalMethod _debug;

        public VueFormMethodsProvider(ContentItem form)
        {
            _getFormContentItem = new GlobalMethod
            {
                Name = "getFormContentItem",
                Method = serviceProvider => (Func<ContentItem>)(() => form)
            };
            _debug = new GlobalMethod
            {
                Name = "debug",
                Method = serviceProvider => (Action<string, object>)((key, obj) => {
                    if(form.As<Models.VueForm>()?.Debug?.Value == true)
                    {
                        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                        httpContextAccessor.HttpContext.Items.TryAdd(Constants.VueFormDebugLog + key, obj);
                    }
                })
            };
            _getSurveyData = new GlobalMethod
            {
                Name = "getSurveyData",
                Method = serviceProvider => (Func<JObject>)(() =>
                {
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    var sanitizer = serviceProvider.GetRequiredService<IHtmlSanitizerService>();

                    var surveyData = httpContextAccessor.HttpContext.Request.Form["__surveyData__"];

                    if(!string.IsNullOrEmpty(surveyData))
                    {
                        return JObject.Parse(sanitizer.Sanitize(surveyData));
                    }
                    return null;
                }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _getFormContentItem, _debug, _getSurveyData };
        }
    }
}
