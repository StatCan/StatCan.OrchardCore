using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentLocalization;
using OrchardCore.ContentManagement;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Scripting
{
    public class LocalizationGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _contentByLocalizationSet;

        public LocalizationGlobalMethodsProvider()
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
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _contentByLocalizationSet };
        }
    }
}
