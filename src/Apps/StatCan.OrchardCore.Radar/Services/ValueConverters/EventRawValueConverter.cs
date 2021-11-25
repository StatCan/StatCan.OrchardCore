using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public class EventRawValueConverter : BaseRawValueConverter
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
            FixSingleArrayValue(rawValues, "attendees[label]");
            FixSingleArrayValue(rawValues, "attendees[value]");
            FixSingleArrayValue(rawValues, "eventOrganizers[label]");
            FixSingleArrayValue(rawValues, "eventOrganizers[value]");

            FillTopics(rawValues);

            var attendees = new JArray();
            if (rawValues["attendees[label]"] != null && rawValues["attendees[value]"] != null)
            {
                for (var i = 0; i < rawValues["attendees[label]"].Count(); i++)
                {
                    var organizerObject = JObject.FromObject(
                         new
                         {
                             label = rawValues["attendees[label]"][i],
                             value = rawValues["attendees[value]"][i]
                         }
                    );

                    attendees.Add(organizerObject);
                }
            }
            rawValues.Remove("attendees[label]");
            rawValues.Remove("attendees[value]");
            rawValues["attendees"] = attendees;

            var eventOrganizers = new JArray();
            if (rawValues["eventOrganizers[label]"] != null && rawValues["eventOrganizers[value]"] != null)
            {
                for (var i = 0; i < rawValues["eventOrganizers[label]"].Count(); i++)
                {
                    var organizerObject = JObject.FromObject(
                         new
                         {
                             label = rawValues["eventOrganizers[label]"][i],
                             value = rawValues["eventOrganizers[value]"][i]
                         }
                    );

                    eventOrganizers.Add(organizerObject);
                }
            }
            rawValues.Remove("eventOrganizers[label]");
            rawValues.Remove("eventOrganizers[value]");
            rawValues["eventOrganizers"] = eventOrganizers;

            return JsonConvert.DeserializeObject<EventFormModel>(rawValues.ToString());
        }
    }
}
