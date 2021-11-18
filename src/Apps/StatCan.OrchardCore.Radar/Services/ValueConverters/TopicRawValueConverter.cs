using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class TopicRawValueConverter : BaseRawValueConverter
    {
        public override FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");

            // Array having a single value gets converted to JValue instead of JArray so we need to convert it back
            if (rawValues["roles"] is JValue)
            {
                var roleArray = new JArray();
                roleArray.Add(rawValues["roles"]);
                rawValues["roles"] = roleArray;
            }

            return JsonConvert.DeserializeObject<TopicFormModel>(rawValues.ToString());
        }
    }
}
