using System.Collections.Generic;
namespace StatCan.OrchardCore.Radar.FormModels
{
    public class CommunityFormModel : EntityFormModel
    {
        public ICollection<IDictionary<string, string>> CommunityMembers { get; set; }

        public string Type { get; set; }
    }
}
