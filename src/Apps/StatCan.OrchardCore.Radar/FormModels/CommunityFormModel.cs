using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class CommunityFormModel : EntityFormModel
    {
        public ICollection<IDictionary<string, object>> CommunityMembers { get; set; }

        public IDictionary<string, string> Type { get; set; }
    }
}
