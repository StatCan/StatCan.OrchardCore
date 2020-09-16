using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Common;
using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Infrastructure.Html;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.VueForms.Scripting
{
    public class VueFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _setModelError;
        private readonly GlobalMethod _formAsJsonObject;
        private readonly GlobalMethod _getFormContentItem;
        private readonly GlobalMethod _debugger;

        public VueFormMethodsProvider(ContentItem form, IDictionary<string, List<string>> errors)
        {
            _setModelError = new GlobalMethod
            {
                Name = "addError",
                Method = serviceProvider =>
                    (Action<string, string>)((name, text) =>
                        {
                            var errorKey = errors.GetOrDefault(name, new List<string>());
                            errorKey.Add(text);
                            if (errors.ContainsKey(name))
                            {
                                errors[name] = errorKey;
                            }
                            else
                            {
                                errors.Add(name, errorKey);
                            }
                           
                        }
                    )
            };

            _debugger = new GlobalMethod
            {
                Name = "debugCode",
                Method = serviceProvider =>
                    (Func<object, object>)(( obj) =>
                    {
                        return obj;
                    }
                    )
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
            return new[] { _setModelError, _formAsJsonObject, _getFormContentItem, _debugger };
        }
    }
}
