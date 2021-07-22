using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace StatCan.Themes.VuetifyTheme
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest _manifest;
        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineScript("vuejs")
                .SetUrl("~/VuetifyTheme/Scripts/vue.min.js", "~/VuetifyTheme/Scripts/vue.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.14/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.14/dist/vue.js")
                .SetCdnIntegrity("sha384-ULpZhk1pvhc/UK5ktA9kwb2guy9ovNSTyxPNHANnA35YjBQgdwI+AhLkixDvdlw4", "sha384-t1tHLsbM7bYMJCXlhr0//00jSs7ZhsAhxgm191xFsyzvieTMCbUWKMhFg9I6ci8q")
                .SetVersion("2.6.14");

            _manifest
                .DefineStyle("vuetify-theme")
                .SetDependencies("vuejs")
                .SetUrl("~/VuetifyTheme/dist/vuetify-theme.css", "~/VuetifyTheme/dist/vuetify-theme.css")
                .SetVersion("1.0.0");
            _manifest
                .DefineScript("vuetify-theme")
                .SetDependencies("vuejs")
                .SetUrl("~/VuetifyTheme/dist/vuetify-theme.umd.min.js", "~/VuetifyTheme/dist/vuetify-theme.umd.js")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
