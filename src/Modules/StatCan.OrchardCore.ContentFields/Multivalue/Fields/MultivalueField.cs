using System;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.ContentFields.Multivalue.Fields
{
    public class MultivalueField : ContentField
    {
        public string[] Values { get; set; } = Array.Empty<string>();
    }
}
