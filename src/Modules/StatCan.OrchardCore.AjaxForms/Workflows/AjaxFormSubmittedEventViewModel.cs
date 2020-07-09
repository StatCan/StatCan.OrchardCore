using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.Workflows.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatCan.OrchardCore.AjaxForms.Workflows
{
    public class AjaxFormSubmittedEventViewModel: ActivityViewModel<AjaxFormSubmittedEvent>
    {
        public AjaxFormSubmittedEventViewModel()
        {

        }
        public AjaxFormSubmittedEventViewModel(AjaxFormSubmittedEvent activity)
        {
            Activity = activity;
        }

        public bool ExecuteForAllForms { get; set; }

        public IList<SelectListItem> AllItems { get; set; } = new List<SelectListItem>();
        public IList<string> SelectedFormIds { get; set; } = new List<string>();

    }
}
