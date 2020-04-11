using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class AjaxForm : ContentPart
    {
        public BooleanField Enabled { get; set; }
        public BooleanField DebugValues { get; set; }
    }
}