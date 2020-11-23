using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentFields.ViewModels;

namespace StatCan.OrchardCore.ContentFields.MultiValueTextField.ViewModels
{
    public class EditMultiValueTextFieldViewModel
    {
        public String[] Values { get; set; }
        public Fields.MultiValueTextField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

        [BindNever]
        public IList<VueMultiselectItemViewModel> SelectedItems { get; set; }
    }
}
