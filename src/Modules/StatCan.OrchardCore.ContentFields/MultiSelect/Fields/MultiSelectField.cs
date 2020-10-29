using OrchardCore.ContentManagement;
using System;

namespace StatCan.OrchardCore.ContentFields.MultiSelect.Fields
{
    public class MultiSelectField : ContentField
    {
        public string[] SelectedValues { get; set; } = Array.Empty<string>();
    }
}
