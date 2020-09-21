using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using OrchardCore.Workflows.ViewModels;

namespace StatCan.OrchardCore.VueForms.Workflows
{
    public class VueFormSubmittedEventViewModel: ActivityViewModel<VueFormSubmittedEvent>
    {
        public VueFormSubmittedEventViewModel()
        {
        }

        public VueFormSubmittedEventViewModel(VueFormSubmittedEvent activity)
        {
            Activity = activity;
        }

        public bool ExecuteForAllForms { get; set; }

        public IList<SelectListItem> AllItems { get; set; } = new List<SelectListItem>();

        public IList<string> SelectedFormIds { get; set; } = new List<string>();
    }
}
