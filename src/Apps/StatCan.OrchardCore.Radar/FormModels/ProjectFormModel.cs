using System.Collections.Generic;

namespace StatCan.OrchardCore.Radar.FormModels
{
    public class ProjectFormModel : EntityFormModel
    {
        public ICollection<IDictionary<string, object>> ProjectMembers { get; set; }

        public IDictionary<string, string> Type { get; set; }
    }
}
