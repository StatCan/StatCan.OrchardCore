using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class EventRawValueConverter : IRawValueConverter
    {
        public FormModel ConvertFromRawValues(JObject rawValues)
        {
            throw new NotImplementedException();
        }
    }
}
