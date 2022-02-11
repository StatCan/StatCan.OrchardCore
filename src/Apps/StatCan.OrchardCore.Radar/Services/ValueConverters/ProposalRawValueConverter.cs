using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class ProposalRawValueConverter : BaseRawValueConverter
    {
        public override FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");
            rawValues.Remove("publishOptions");
            rawValues.Remove("typeOptions[label]");
            rawValues.Remove("typeOptions[value]");

            FixSingleArrayValue(rawValues, "roles");
            FixSingleArrayValue(rawValues, "topics[label]");
            FixSingleArrayValue(rawValues, "topics[value]");
            FixSingleArrayValue(rawValues, "relatedEntities[label]");
            FixSingleArrayValue(rawValues, "relatedEntities[value]");

            FillTopics(rawValues);
            FillRelatedEntities(rawValues);
            FillType(rawValues);

            return JsonConvert.DeserializeObject<ProposalFormModel>(rawValues.ToString());
        }
    }
}
