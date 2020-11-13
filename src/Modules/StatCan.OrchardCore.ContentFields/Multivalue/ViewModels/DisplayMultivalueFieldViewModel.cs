using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using StatCan.OrchardCore.ContentFields.Multivalue.Fields;

namespace StatCan.OrchardCore.ContentFields.Multivalue.ViewModels
{
    public class DisplayMultivalueFieldViewModel
    {
        public string[] Values => Field.Values;
        public MultivalueField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }
    }
}
