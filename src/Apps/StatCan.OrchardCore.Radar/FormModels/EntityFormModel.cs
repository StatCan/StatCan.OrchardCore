using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class EntityFormModel : FormModel
    {
        public string[] Topics { get; set; }
        public ICollection<string> RelatedEntities { get; set; }
    }
}
