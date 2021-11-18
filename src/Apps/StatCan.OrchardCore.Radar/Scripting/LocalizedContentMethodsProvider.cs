using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Scripting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentLocalization;
using OrchardCore.Localization;

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class LocalizedContentMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _createLocalizedContentItem;

        public LocalizedContentMethodsProvider()
        {
            _createLocalizedContentItem = new GlobalMethod()
            {
                Name = "_createLocalizedContentItem",
                Method = serviceProvider => (Func<string, bool, JObject, IContent>)((contentType, publish, properties) =>
                {
                    // The behaviour is that a content will be created in the default culture. Then the content will be localized across all supported localizations

                    // Creating content in the default culture
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    var contentItem = contentManager.NewAsync(contentType).GetAwaiter().GetResult();
                    contentItem.Merge(properties);
                    var result = contentManager.UpdateValidateAndCreateAsync(contentItem, publish == true ? VersionOptions.Published : VersionOptions.Draft).GetAwaiter().GetResult();

                    // Create localized version of the content
                    var contentLocalizationManager = serviceProvider.GetRequiredService<IContentLocalizationManager>();
                    var localizationService = serviceProvider.GetRequiredService<ILocalizationService>();

                    var supportedCultures = localizationService.GetSupportedCulturesAsync().GetAwaiter().GetResult();

                    foreach (var culture in supportedCultures)
                    {
                        contentLocalizationManager.LocalizeAsync(contentItem, culture);
                    }

                    return contentItem;
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createLocalizedContentItem };
        }

    }
}
