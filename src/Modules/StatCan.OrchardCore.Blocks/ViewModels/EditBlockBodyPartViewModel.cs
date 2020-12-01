using Etch.OrchardCore.Blocks.Models;
using OrchardCore.ContentManagement.Metadata.Models;

namespace Etch.OrchardCore.Blocks.ViewModels
{
    public class EditBlockBodyPartViewModel
    {
        public BlockBodyPart Part { get; set; }
        public ContentTypePartDefinition TypePartDefinition { get; set; }

        public string Data { get; set; }
    }
}
