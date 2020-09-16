using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Scripting;
using StatCan.OrchardCore.LocalizedText.Fields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace StatCan.OrchardCore.LocalizedText.Scripting
{
    public class GlobalMethodProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _getLocalizedValues;

        public GlobalMethodProvider()
        {
            _getLocalizedValues = new GlobalMethod
            {
                // return a JObject that contains every name -> value pairs from the LocalizedTextPart
                // with values of the current culture
                Name = "getLocalizedTextValues",
                Method = serviceProvider => (Func<ContentItem, JObject>)((item) =>
                {
                    var culture = CultureInfo.CurrentUICulture;

                    var localizedTextPart = item.As<LocalizedTextPart>();

                    var jobj = new JObject();
                    if (localizedTextPart != null)
                    {
                        foreach (var le in localizedTextPart.Data)
                        {
                            jobj.Add(le.Name, new JValue(le.LocalizedItems.FirstOrDefault(l => l.Culture == culture.Name)?.Value));
                        }
                    }
                    return jobj;
                })
            };

        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _getLocalizedValues };
        }
    }
}


