using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class ProjectRawValueConverter : IRawValueConverter
    {
        public FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");
            rawValues.Remove("visibilityOptions");
            rawValues.Remove("typeOptions[label]");
            rawValues.Remove("typeOptions[value]");
            rawValues.Remove("valueNames");

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

            var topics = new JArray();
            for (var i = 0; i < rawValues["topics[label]"].Count(); i++)
            {
                var topicObject = JObject.FromObject(
                    new
                    {
                        value = rawValues["topics[value]"][i],
                        label = rawValues["topics[label]"][i]
                    }
                );

                topics.Add(topicObject);
            }
            rawValues.Remove("topics[label]");
            rawValues.Remove("topics[value]");
            rawValues["topics"] = topics;


            var type = JObject.FromObject(
                new
                {
                    label = rawValues["type[label]"],
                    value = rawValues["type[value"]
                }
            );
            rawValues.Remove("type[label]");
            rawValues.Remove("type[value]");
            rawValues["type"] = type;

            return JsonConvert.DeserializeObject<ProjectFormModel>(rawValues.ToString());
        }
    }
}
