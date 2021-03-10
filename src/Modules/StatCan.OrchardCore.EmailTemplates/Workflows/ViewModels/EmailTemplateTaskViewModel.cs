using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.EmailTemplates.Workflows.ViewModels
{
    public class EmailTemplateTaskViewModel
    {
        public string TemplateModelExpression { get; set; }
        public List<SelectListItem> EmailTemplates { get; set; }
        public string SelectedEmailTemplateId { get; set; }
    }
}
