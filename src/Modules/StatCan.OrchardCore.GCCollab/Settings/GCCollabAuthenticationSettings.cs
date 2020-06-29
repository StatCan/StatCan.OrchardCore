using Microsoft.AspNetCore.Http;

namespace StatCan.OrchardCore.GCCollab.Settings
{
    public class GCCollabAuthenticationSettings
    {
        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public PathString CallbackPath { get; set; }
    }
}
