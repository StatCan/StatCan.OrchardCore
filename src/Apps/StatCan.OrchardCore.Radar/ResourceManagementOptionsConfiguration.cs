using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace StatCan.OrchardCore.Radar
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest _manifest;

        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineStyle("Radar-styles")
                .SetUrl("~/StatCan.OrchardCore.Radar/css/radar.min.css", "~/StatCan.OrchardCore.Radar/css/radar.css")
                .SetVersion("1.0.0");

            _manifest
                .DefineScript("Radar-vue-components")
                .SetDependencies("vuetify-theme")
                .SetUrl("~/StatCan.OrchardCore.Radar/js/vue-components/radar-vue-components.umd.min.js", "~/StatCan.OrchardCore.Radar/js/vue-components/radar-vue-components.umd.js")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
