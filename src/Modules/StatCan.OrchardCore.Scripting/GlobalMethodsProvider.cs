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
        private readonly GlobalMethod _contentByLocalizationSet;
        private readonly GlobalMethod _contentByItemId;

        public GlobalMethodsProvider(IHttpContextAccessor httpContextAccessor)
        {
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
            return new[] { _contentByLocalizationSet, _contentByItemId };
        }
    }
}
