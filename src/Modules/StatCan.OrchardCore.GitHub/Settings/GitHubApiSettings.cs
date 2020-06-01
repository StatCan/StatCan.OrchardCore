using Newtonsoft.Json;

namespace StatCan.OrchardCore.GitHub.Settings
{
    public class GitHubApiSettings
    {
        public ApiToken[] ApiTokens { get; set; } = new ApiToken[0];
    }

    public class ApiToken
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
