using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class AjaxForm : ContentPart
    {
        public BooleanField Enabled { get; set; }

        // todo: feature that helps debug forms in the admin ui
       // public BooleanField DebugValues { get; set; }
        //public BooleanField TriggerWorkflow { get; set; }

    }
}