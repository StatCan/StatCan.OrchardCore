using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;
using OrchardCore.ReCaptcha.Services;
using OrchardCore.DisplayManagement.ModelBinding;

namespace StatCan.OrchardCore.Scripting
{
    public class FormsGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _formAsJsonObject;
        private readonly GlobalMethod _validateReCaptcha;
        private readonly GlobalMethod _setModelError;
        private readonly GlobalMethod _hasErrors;

        public FormsGlobalMethodsProvider()
        {
             _setModelError = new GlobalMethod
            {
                Name = "addError",
                Method = serviceProvider => (Action<string, string>)((name, text) => {
                   var updateModelAccessor = serviceProvider.GetRequiredService<IUpdateModelAccessor>();
                    updateModelAccessor.ModelUpdater.ModelState.AddModelError(name, text);
                })
            };
            _hasErrors = new GlobalMethod
            {
                Name = "hasErrors",
                Method = serviceProvider => (Func<bool>)(() => {
                   var updateModelAccessor = serviceProvider.GetRequiredService<IUpdateModelAccessor>();
                    return updateModelAccessor.ModelUpdater.ModelState.ErrorCount > 0;
                })
            };
            _formAsJsonObject = new GlobalMethod
            {
                Name = "requestFormAsJsonObject",
                Method = serviceProvider => (Func<JObject>)(() =>
                {
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    var sanitizer = serviceProvider.GetRequiredService<IHtmlSanitizerService>();

                    return new JObject(httpContextAccessor.HttpContext.Request.Form.Select(
                      field =>
                      {
                          var arr = field.Value.ToArray();
                          if (arr.Length == 1)
                          {
                              return new JProperty(field.Key, sanitizer.Sanitize(field.Value[0]));
                          }
                          return new JProperty(field.Key, JArray.FromObject(arr.Select(o => sanitizer.Sanitize(o))));
                      }
                    ).ToArray());
                }
                )
            };
            _validateReCaptcha = new GlobalMethod
            {
                Name = "validateReCaptcha",
                Method = serviceProvider => (Func<string, bool>)((reCaptchaResponse) => {
                    var recaptchaService = serviceProvider.GetRequiredService<ReCaptchaService>();
                    return recaptchaService.VerifyCaptchaResponseAsync(reCaptchaResponse).GetAwaiter().GetResult();
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _formAsJsonObject, _validateReCaptcha, _setModelError, _hasErrors };
        }
    }
}
