using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Scripting;
using YesSql;

namespace StatCan.OrchardCore.Scripting
{
    public class ContentGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _contentByItemId;
        private readonly GlobalMethod _contentByItemIdVersion;
        private readonly GlobalMethod _ownContentByType;
        private readonly GlobalMethod _contentByVersionId;

        public ContentGlobalMethodsProvider(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            _contentByItemId = new GlobalMethod
            {
                Name = "contentByItemId",
                Method = serviceProvider => (Func<string, ContentItem>)((contentItemId) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    return contentManager.GetAsync(contentItemId).GetAwaiter().GetResult();
                })
            };

            _contentByItemIdVersion = new GlobalMethod
            {
                Name = "contentByItemIdVersion",
                Method = serviceProvider => (Func<string, string, ContentItem>)((contentItemId, version) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    return version.ToLower() switch
                    {
                        "published" => contentManager.GetAsync(contentItemId, VersionOptions.Published).GetAwaiter().GetResult(),
                        "latest" => contentManager.GetAsync(contentItemId, VersionOptions.Latest).GetAwaiter().GetResult(),
                        "draft" => contentManager.GetAsync(contentItemId, VersionOptions.Draft).GetAwaiter().GetResult(),
                        "draftrequired" => contentManager.GetAsync(contentItemId, VersionOptions.DraftRequired).GetAwaiter().GetResult(),
                        _ => contentManager.GetAsync(contentItemId).GetAwaiter().GetResult(),
                    };
                })
            };

            _contentByVersionId = new GlobalMethod
            {
                Name = "contentByVersionId",
                Method = serviceProvider => (Func<string, ContentItem>)((contentItemVersionId) =>
                {
                    var contentManager = serviceProvider.GetRequiredService<IContentManager>();
                    return contentManager.GetVersionAsync(contentItemVersionId).GetAwaiter().GetResult();
                })
            };

            _ownContentByType = new GlobalMethod
            {
                Name = "ownContentByType",
                Method = serviceProvider => (Func<string, IEnumerable<ContentItem>>)((type) =>
                {
                    var session = serviceProvider.GetRequiredService<ISession>();
                    var owner = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    return session.Query<ContentItem, ContentItemIndex>(c=>c.Owner == owner && c.ContentType == type && c.Published).ListAsync().GetAwaiter().GetResult();
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _contentByItemId, _ownContentByType, _contentByVersionId, _contentByItemIdVersion };
        }
    }
}
