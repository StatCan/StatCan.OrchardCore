using Etch.OrchardCore.Blocks.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;

namespace Etch.OrchardCore.Blocks.ViewModels
{
    public class DisplayBlockFieldViewModel
    {
        public BlockField Field { get; set; }
        public ContentPart Part { get; set; }
        public ContentPartFieldDefinition PartFieldDefinition { get; set; }

        public string Html { get; set; }

        public IList<dynamic> Blocks { get; set; }
    }
}
