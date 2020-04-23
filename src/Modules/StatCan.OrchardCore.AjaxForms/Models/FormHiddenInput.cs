using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class FormHiddenInput: ContentPart
    {
        public TextField Name { get; set; }
        public TextField Value { get; set; }
    }
}
