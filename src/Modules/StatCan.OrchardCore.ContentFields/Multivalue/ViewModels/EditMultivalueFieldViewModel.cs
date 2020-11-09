using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentFields.ViewModels;
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;

namespace StatCan.OrchardCore.ContentFields.Multivalue.ViewModels
{
    public class EditMultivalueFieldViewModel
    {
        public string Values { get; set; }
        public MultivalueField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

        [BindNever]
        public IList<VueMultiselectItemViewModel> SelectedItems { get; set; }
    }
}
