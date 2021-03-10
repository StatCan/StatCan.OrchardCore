using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace StatCan.OrchardCore.EmailTemplates.Workflows.ViewModels
{
    public class EmailTemplateTaskViewModel
    {
        public string TemplateModelExpression { get; set; }
        public List<SelectListItem> EmailTemplates { get; set; }
        public string SelectedEmailTemplateId { get; set; }
    }
}
