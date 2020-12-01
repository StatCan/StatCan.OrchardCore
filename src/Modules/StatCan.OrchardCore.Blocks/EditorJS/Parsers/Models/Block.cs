using Newtonsoft.Json;
using System.Collections.Generic;

namespace Etch.OrchardCore.Blocks.EditorJS.Parsers.Models
{
    public class Block
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public IDictionary<string, object> Data { get; set; }

        public T Get<T>(string property, T defaultValue)
        {
            return Has(property) ? (T)Data[property] : defaultValue;
        }

        public string Get(string property)
        {
            return Has(property) ? Data[property]?.ToString() : string.Empty;
        }

        public bool Has(string property)
        {
            return Data.ContainsKey(property);
        }
    }
}
