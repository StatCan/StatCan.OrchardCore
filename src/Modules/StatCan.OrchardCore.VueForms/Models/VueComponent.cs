using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace StatCan.OrchardCore.VueForms.Models
{
    public class VueComponent: ContentPart
    {
        public TextField Template { get; set; }
    }
}
