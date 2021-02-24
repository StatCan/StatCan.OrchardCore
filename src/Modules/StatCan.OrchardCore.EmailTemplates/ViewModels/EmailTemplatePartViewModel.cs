using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatCan.OrchardCore.EmailTemplates.ViewModels
{
    public class EmailTemplatePartViewModel
    {
        public List<SelectListItem> SelectedEmailTemplates { get; set; }
        public string ContentItemId { get; set; }
    }
}
