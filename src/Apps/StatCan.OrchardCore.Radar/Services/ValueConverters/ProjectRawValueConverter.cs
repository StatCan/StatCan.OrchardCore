using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class ProjectRawValueConverter : BaseRawValueConverter
    {
        public override FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");
            rawValues.Remove("visibilityOptions");
            rawValues.Remove("typeOptions[label]");
            rawValues.Remove("typeOptions[value]");
            rawValues.Remove("valueNames");

            FixSingleArrayValue(rawValues, "roles");
            FixSingleArrayValue(rawValues, "topics[label]");
            FixSingleArrayValue(rawValues, "topics[value]");
            FixSingleArrayValue(rawValues, "projectMembers[role]");
            FixSingleArrayValue(rawValues, "projectMembers[user][label]");
            FixSingleArrayValue(rawValues, "projectMembers[user][value]");

            FillTopics(rawValues);
            FillType(rawValues);

            // Convert to project form model
            // Normalize project member
            var projectMembers = new JArray();
            for (var i = 0; i < rawValues["projectMembers[role]"].Count(); i++)
            {
                var memberObject = JObject.FromObject(
                     new
                     {
                         role = rawValues["projectMembers[role]"][i],
                         user = new
                         {
                             label = rawValues["projectMembers[user][label]"][i],
                             value = rawValues["projectMembers[user][value]"][i]
                         }
                     }
                );

                projectMembers.Add(memberObject);
            }
            rawValues.Remove("projectMembers[role]");
            rawValues.Remove("projectMembers[user][label]");
            rawValues.Remove("projectMembers[user][value]");
            rawValues["projectMembers"] = projectMembers;

            return JsonConvert.DeserializeObject<ProjectFormModel>(rawValues.ToString());
        }
    }
}
