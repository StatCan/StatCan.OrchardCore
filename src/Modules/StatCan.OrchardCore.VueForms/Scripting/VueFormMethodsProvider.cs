using System;
using System.Collections.Generic;
using AngleSharp.Common;
using OrchardCore.ContentManagement;
using OrchardCore.Scripting;

namespace StatCan.OrchardCore.VueForms.Scripting
{
    public class VueFormMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _setModelError;
        private readonly GlobalMethod _getFormContentItem;

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
            _getFormContentItem = new GlobalMethod
            {
                Name = "getFormContentItem",
                Method = serviceProvider => (Func<ContentItem>)(() => form)
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _setModelError, _getFormContentItem };
        }
    }
}
