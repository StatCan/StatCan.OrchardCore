using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.ContentLocalization;
using OrchardCore.ContentManagement;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Scripting
{
    public class GlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _formAsJsonObject;
        private readonly GlobalMethod _formFieldAsJsonObject;
        private readonly GlobalMethod _contentByLocalizationSet;
        private readonly GlobalMethod _contentByItemId;

        public GlobalMethodsProvider(IHttpContextAccessor httpContextAccessor)
        {
            _formAsJsonObject = new GlobalMethod
            {
                Name = "requestFormAsJsonObject",
                Method = serviceProvider => (Func<JObject>)(() =>
                    new JObject(httpContextAccessor.HttpContext.Request.Form.Select(
                      field =>
                      {
                          var arr = field.Value.ToArray();
                          if (arr.Length == 1)
                          {
                              return new JProperty(field.Key, field.Value[0]);
                          }
                          return new JProperty(field.Key, JArray.FromObject(arr));
                      }
                    ).ToArray())
                  )

            };
            _formFieldAsJsonObject = new GlobalMethod
            {
                Name = "requestFormFieldAsJsonObject",
                Method = serviceProvider => (Func<string,JObject>)((fieldName) => {
                    var obj = JObject.Parse(httpContextAccessor.HttpContext.Request.Form.FirstOrDefault(f => f.Key.Equals(fieldName)).Value);
                    return obj;
                    }
                  )

            };
            _contentByLocalizationSet = new GlobalMethod
            {
                Name = "contentByLocalizationSet",
                Method = serviceProvider => (Func<string, string, ContentItem>)((localizationSet, culture) =>
                  {
                      var localizationService = serviceProvider.GetRequiredService<IContentLocalizationManager>();
                      return localizationService.GetContentItemAsync(localizationSet, culture).GetAwaiter().GetResult();
                  }
                )
            };
            _contentByItemId = new GlobalMethod
            {
                Name = "contentByItemId",
                Method = serviceProvider => (Func<string, ContentItem>)((contentItemId) =>
                  {
                      var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                      return contentManager.GetAsync(contentItemId).GetAwaiter().GetResult();
                  }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _formAsJsonObject, _formFieldAsJsonObject, _contentByLocalizationSet, _contentByItemId };
        }
    }
}
