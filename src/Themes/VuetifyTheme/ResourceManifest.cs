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
                .DefineStyle("vuetifytheme-styles")
                .SetUrl("~/VuetifyTheme/dist/vuetify-theme.css", "~/VuetifyTheme/dist/vuetify-theme.css")
                .SetVersion("1.0.0");
            _manifest
                .DefineScript("vuejs")
                .SetUrl("~/VuetifyTheme/Scripts/vue.min.js", "~/VuetifyTheme/Scripts/vue.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.js")
                .SetCdnIntegrity("sha384-cwVe6U8Tq7F/3JIj6xeDzOwuqeChcmRcdYqDGfoYmdAurw7L3f4dFHhEJKfxv96A", "sha384-ma9ivURrHX5VOB4tNq+UiGNkJoANH4EAJmhxd1mmDq0gKOv88wkKZOfRDOpXynwh")
                .SetVersion("2.6.12");
            _manifest
                .DefineScript("vuetifytheme-scripts")
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
