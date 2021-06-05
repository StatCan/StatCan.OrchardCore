using System;
using System.Collections.Generic;
using AngleSharp.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.VueForms.Scripting
{
    /// <summary>
    /// This does not need to be registered in the DI as these methods are only used in the VueFormController
    /// </summary>
    public class VueFormMethodsProvider : IGlobalMethodProvider
    {

        private readonly GlobalMethod _getFormContentItem;

        public VueFormMethodsProvider(ContentItem form)
        {
            _getFormContentItem = new GlobalMethod
            {
                Name = "getFormContentItem",
                Method = serviceProvider => (Func<ContentItem>)(() => form)
            };
            _getFormContentItem = new GlobalMethod
            {
                Name = "addSuccessMessage",
                Method = serviceProvider => (Action<string>)((message) => {
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    httpContextAccessor.HttpContext.Items.Add(Constants.VueFormSuccessMessage, message);
                })
            };
            _getFormContentItem = new GlobalMethod
            {
                Name = "debugLog",
                Method = serviceProvider => (Action<string, object>)((key, obj) => {
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    httpContextAccessor.HttpContext.Items.TryAdd(Constants.VueFormDebugLog + key, obj.ToString());
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _getFormContentItem };
        }
    }
}
