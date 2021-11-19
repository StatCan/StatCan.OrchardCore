using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Scripting;
using OrchardCore.ContentManagement;
using OrchardCore.ContentLocalization;
using OrchardCore.Localization;
using OrchardCore.Autoroute.Models;

namespace StatCan.OrchardCore.Radar.Scripting
{
    public class LocalizedContentMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _createLocalizedContentItem;
        private readonly GlobalMethod _updateLocalizedContentItem;
        private readonly GlobalMethod _getLocalizedContentItemById;

        public LocalizedContentMethodsProvider()
        {
            _createLocalizedContentItem = new GlobalMethod()
            {
                Name = "createLocalizedContentItem",
                Method = serviceProvider => (Func<string, JObject, IContent>)((contentType, properties) =>
                {
                    // The behaviour is that a content will be created in the default culture. Then the content will be localized across all supported localizations

                    bool publish = properties["Published"].Value<bool>();

                    properties.Remove("Published");

                    // Creating content in the default culture
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    var contentItem = contentManager.NewAsync(contentType).GetAwaiter().GetResult();
                    contentItem.Merge(properties);

                    // Create localized version of the content
                    var contentLocalizationManager = serviceProvider.GetRequiredService<IContentLocalizationManager>();
                    var localizationService = serviceProvider.GetRequiredService<ILocalizationService>();

                    var supportedCultures = localizationService.GetSupportedCulturesAsync().GetAwaiter().GetResult();
                    ContentItem resultItem = null;

                    foreach (var culture in supportedCultures)
                    {
                        var localizedContent = contentLocalizationManager.LocalizeAsync(contentItem, culture).GetAwaiter().GetResult();
                        contentManager.CreateAsync(localizedContent).GetAwaiter().GetResult();
                        contentManager.UpdateAsync(localizedContent).GetAwaiter().GetResult(); // This is needed to to generate the routes

                        localizedContent.Alter<AutoroutePart>(part => part.RouteContainedItems = true);
                        contentManager.UpdateAsync(localizedContent).GetAwaiter().GetResult(); // This is needed to enable the rounte contained option

                        if (publish)
                        {
                            contentManager.PublishAsync(localizedContent).GetAwaiter().GetResult();
                        }

                        if(CultureInfo.CurrentCulture.Name == culture)
                        {
                            resultItem = localizedContent;
                        }
                    }

                    return resultItem;
                })
            };

            _updateLocalizedContentItem = new GlobalMethod()
            {
                Name = "updateLocalizedContentItem",
                Method = serviceProvider => (Action<ContentItem, JObject>)((contentItem, properties) =>
                {
                    bool publish = properties["Published"].Value<bool>();

                    // Creating content in the default culture
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    contentItem.Merge(properties, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Replace });
                    contentManager.UpdateAsync(contentItem).GetAwaiter().GetResult();

                    if (publish)
                    {
                        contentManager.PublishAsync(contentItem).GetAwaiter().GetResult();
                    }
                    else
                    {
                        contentManager.UnpublishAsync(contentItem).GetAwaiter().GetResult();
                    }
                })
            };

            _getLocalizedContentItemById = new GlobalMethod()
            {
                Name = "getLocalizedContentItemById",
                Method = serviceProvider => (Func<string, ContentItem>)((id) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    var contentLocalizationManager = serviceProvider.GetRequiredService<IContentLocalizationManager>();

                    var contentItem = contentManager.GetAsync(id, VersionOptions.Latest).GetAwaiter().GetResult();

                    if (contentItem == null)
                    {
                        return null;
                    }

                    var localizationSet = contentItem.Content.LocalizationPart.LocalizationSet.ToString();
                    var localizedContent = contentLocalizationManager.GetContentItemAsync(localizationSet, CultureInfo.CurrentCulture.Name).GetAwaiter().GetResult();

                    return localizedContent;
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _createLocalizedContentItem, _updateLocalizedContentItem, _getLocalizedContentItemById };
        }

    }
}
