using System;

namespace StatCan.OrchardCore.SaaSConfiguration
{
    public class SaaSConfigurationSettings
    {
        public Uri Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
