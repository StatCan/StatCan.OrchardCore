using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class ArtifactRawValueConverter : BaseRawValueConverter
    {
        public override FormModel ConvertFromRawValues(JObject rawValues)
        {
            rawValues.Remove("roleOptions");
            rawValues.Remove("__RequestVerificationToken");

            FixSingleArrayValue(rawValues, "roles");

            return JsonConvert.DeserializeObject<ArtifactFormModel>(rawValues.ToString());
        }
    }
}
