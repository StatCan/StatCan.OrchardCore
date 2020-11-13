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
        private readonly GlobalMethod _ownContentByType;

        public ContentGlobalMethodsProvider(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
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
            _ownContentByType = new GlobalMethod
            {
                Name = "ownContentByType",
                Method = serviceProvider => (Func<string, ContentItem>)((type) =>
                {
                    var session = serviceProvider.GetRequiredService<ISession>();
                    var owner = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                    return session.Query<ContentItem, ContentItemIndex>(c=>c.Owner == owner && c.ContentType == type && c.Published == true).FirstOrDefaultAsync().GetAwaiter().GetResult();
                }
                )
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _contentByItemId, _ownContentByType };
        }
    }
}
