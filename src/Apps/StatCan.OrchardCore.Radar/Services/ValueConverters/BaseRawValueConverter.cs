using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StatCan.OrchardCore.Radar.FormModels;

namespace StatCan.OrchardCore.Radar.Services.ValueConverters
{
    public abstract class BaseRawValueConverter : IRawValueConverter
    {
        public Task<FormModel> ConvertAsync(JObject rawValues)
        {
            return ConvertFromRawValuesAsync(rawValues);
        }

        public virtual FormModel ConvertFromRawValues(JObject rawValues)
        {
            return null;
        }

        public virtual Task<FormModel> ConvertFromRawValuesAsync(JObject rawValues)
        {
            return Task.FromResult(ConvertFromRawValues(rawValues));
        }

        protected void FixSingleArrayValue(JObject rawValues, string key)
        {
            // Array having a single value gets converted to JValue instead of JArray so we need to convert it back
            if (rawValues[key] is JValue)
            {
                var roleArray = new JArray
                {
                    rawValues[key]
                };
                rawValues[key] = roleArray;
            }
        }

        protected void FillTopics(JObject rawValues)
        {
            var topics = new JArray();
            if(rawValues["topics[label]"] != null && rawValues["topics[label]"] != null)
            {
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
            }
            rawValues["topics"] = topics;
        }

        protected void FillType(JObject rawValues)
        {
            var type = JObject.FromObject(
                new
                {
                    label = rawValues["type[label]"],
                    value = rawValues["type[value]"]
                }
            );
            rawValues.Remove("type[label]");
            rawValues.Remove("type[value]");
            rawValues["type"] = type;
        }

        protected void FillRelatedEntities(JObject rawValues)
        {
            var relatedEntities = new JArray();
            if(rawValues["relatedEntities[label]"] != null && rawValues["relatedEntities[label]"] != null)
            {
                for (var i = 0; i < rawValues["relatedEntities[label]"].Count(); i++)
                {
                    var topicObject = JObject.FromObject(
                        new
                        {
                            value = rawValues["relatedEntities[value]"][i],
                            label = rawValues["relatedEntities[label]"][i]
                        }
                    );

                    relatedEntities.Add(topicObject);
                }
            }
            rawValues.Remove("relatedEntities[label]");
            rawValues.Remove("relatedEntities[value]");
            rawValues["relatedEntities"] = relatedEntities;
        }
    }
}
