using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class ProjectFormModel : EntityFormModel
    {
        public ICollection<IDictionary<string, string>> ProjectMembers { get; set; }

        public string Type { get; set; }
    }
}
