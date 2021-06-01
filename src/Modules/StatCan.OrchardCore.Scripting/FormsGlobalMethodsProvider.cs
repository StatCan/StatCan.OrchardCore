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
using OrchardCore.DisplayManagement.Notify;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Media;
using System.IO;
using Microsoft.Extensions.Localization;
using OrchardCore.FileStorage;
using Microsoft.Extensions.Logging;

namespace StatCan.OrchardCore.Scripting
{
    public class FormsGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _formAsJsonObject;
        private readonly GlobalMethod _validateReCaptcha;
        private readonly GlobalMethod _setModelError;
        private readonly GlobalMethod _hasErrors;
        private readonly GlobalMethod _notify;

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

            _notify = new GlobalMethod
            {
                Name = "notify",
                Method = serviceProvider => (Func<string, string, bool>)((type, message ) => {
                    var notifyService = serviceProvider.GetRequiredService<INotifier>();
                    if(Enum.TryParse(typeof(NotifyType), type, out var notifyType))
                    {
                        notifyService.Add((NotifyType)notifyType, new LocalizedHtmlString(nameof(FormsGlobalMethodsProvider), message));
                        return true;
                    }
                    return false;
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _formAsJsonObject, _validateReCaptcha, _setModelError, _hasErrors, _notify };
        }
    }
}
