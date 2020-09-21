using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.Scripting
{
    public class ContentGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _contentByItemId;

        public ContentGlobalMethodsProvider(IHttpContextAccessor httpContextAccessor)
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
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _contentByItemId };
        }
    }
}
