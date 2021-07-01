using StatCan.OrchardCore.Blocks.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;

namespace StatCan.OrchardCore.Blocks.ViewModels
{
    public class EditBlockFieldViewModel
    {
        public BlockField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

        public string Data { get; set; }
    }
}
