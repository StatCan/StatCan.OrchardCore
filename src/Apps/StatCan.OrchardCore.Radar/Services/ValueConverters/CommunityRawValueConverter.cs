using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class CommunityRawValueConverter : BaseRawValueConverter
    {
        public override FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");
            rawValues.Remove("publishOptions");
            rawValues.Remove("typeOptions[label]");
            rawValues.Remove("typeOptions[value]");
            rawValues.Remove("valueNames");

            FixSingleArrayValue(rawValues, "roles");
            FixSingleArrayValue(rawValues, "topics[label]");
            FixSingleArrayValue(rawValues, "topics[value]");
            FixSingleArrayValue(rawValues, "relatedEntities[label]");
            FixSingleArrayValue(rawValues, "relatedEntities[value]");
            FixSingleArrayValue(rawValues, "communityMembers[role]");
            FixSingleArrayValue(rawValues, "communityMembers[user][label]");
            FixSingleArrayValue(rawValues, "communityMembers[user][value]");

            FillTopics(rawValues);
            FillRelatedEntities(rawValues);
            FillType(rawValues);

            // Convert to project form model
            // Normalize project member
            var communityMembers = new JArray();
            if (rawValues["communityMembers[role]"] != null && rawValues["communityMembers[user][label]"] != null && rawValues["communityMembers[user][value]"] != null)
            {
                for (var i = 0; i < rawValues["communityMembers[role]"].Count(); i++)
                {
                    var memberObject = JObject.FromObject(
                         new
                         {
                             role = rawValues["communityMembers[role]"][i],
                             user = new
                             {
                                 label = rawValues["communityMembers[user][label]"][i],
                                 value = rawValues["communityMembers[user][value]"][i]
                             }
                         }
                    );

                    communityMembers.Add(memberObject);
                }
            }
            rawValues.Remove("communityMembers[role]");
            rawValues.Remove("communityMembers[user][label]");
            rawValues.Remove("communityMembers[user][value]");
            rawValues["communityMembers"] = communityMembers;

            return JsonConvert.DeserializeObject<CommunityFormModel>(rawValues.ToString());
        }
    }
}
