using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.Environment.Shell;

namespace StatCan.OrchardCore.SaaSConfiguration.ViewModels
{
    public class ExecuteRecipeViewModel
    {
        public string Json { get; set; }

        public IList<string> SelectedTenantNames { get; set; } = new List<string>();

        [BindNever]
        public IList<SelectListItem> AllTenants { get; set; } = new List<SelectListItem>();

    }
}
