using StatCan.OrchardCore.LocalizedText.Models;
using OrchardCore.ContentManagement;
using System.Collections.Generic;

namespace StatCan.OrchardCore.LocalizedText.Fields
{
    public class LocalizedTextPart : ContentPart
    {
        public IList<LocalizedTextEntry> Data { get; set; }
    }
}
