using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatCan.OrchardCore.EmailTemplates.Models;
using System.Collections.Generic;

namespace StatCan.OrchardCore.EmailTemplates.ViewModels
{
    public class EmailTemplatePartSettingsViewModel
    {
        public List<SelectListItem> EmailTemplates { get; set; }
        public string EmailTemplate { get; set; }
        public EmailTemplatePartSettings EmailTemplatePartSettings { get; set; }
    }
}
