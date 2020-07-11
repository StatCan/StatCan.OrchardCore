using System;
using System.Collections.Generic;
using System.Linq;
using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;

namespace OrchardCore.Workflows.Scripting
{
    public class AjaxFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _setModelError;
        private readonly GlobalMethod _formAsJsonObject;
        private readonly GlobalMethod _getFormContentItem;

        public AjaxFormMethodsProvider(ContentItem form, IUpdateModel modelUpdater)
        {
            _setModelError = new GlobalMethod
            {
                Name = "addModelError",
                Method = serviceProvider =>
                    (Func<string, string, bool>)((name, text) =>
                        modelUpdater.ModelState.TryAddModelError(name, text))
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
            _getFormContentItem = new GlobalMethod
            {
                Name = "getFormContentItem",
                Method = serviceProvider => (Func<ContentItem>)(() => form)
            };

        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _setModelError, _formAsJsonObject, _getFormContentItem };
        }
    }
}
