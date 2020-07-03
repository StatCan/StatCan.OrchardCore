using System.Collections;
using System.Collections.Generic;

namespace StatCan.OrchardCore.LocalizedText.Models
{
    public class LocalizedTextEntry
    {
        public string Name { get; set; }
        public IList<LocalizedTextItem> LocalizedItems { get; set; }
    }

    public class LocalizedTextItem
    {
        public string Culture { get; set; }
        public string Value { get; set; }
    }
}
