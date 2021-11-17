using System.Collections;
using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class EntityFormModel : FormModel
    {
        public ICollection<IDictionary<string, string>> Topics { get; set; }
        public ICollection<string> RelatedEntities { get; set; }

        public string Visibility { get; set; }
    }
}