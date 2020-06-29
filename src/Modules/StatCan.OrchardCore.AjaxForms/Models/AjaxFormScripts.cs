using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.AjaxForms.Models
{
    public class AjaxFormScripts: ContentPart
    {
        public TextField ValidationScript { get; set; }
        public TextField OnChangeScript { get; set; }
    }
}
