using Etch.OrchardCore.Blocks.Models;
using OrchardCore.ContentManagement.Metadata.Models;
using System.Collections.Generic;

namespace Etch.OrchardCore.Blocks.ViewModels
{
    public class DisplayBlockBodyPartViewModel
    {
        public IList<dynamic> Blocks { get; set; }

        public BlockBodyPart Part { get; set; }

        public ContentTypePartDefinition TypePartDefinition { get; set; }
    }
}
