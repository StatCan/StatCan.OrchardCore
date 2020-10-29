using StatCan.OrchardCore.ContentFields.MultiSelect.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System;

namespace StatCan.OrchardCore.ContentFields.MultiSelect.ViewModels
{
    public class EditMultiSelectFieldViewModel
    {
        public MultiSelectField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

        public string[] SelectedValues { get; set; } = Array.Empty<string>();
    }
}
