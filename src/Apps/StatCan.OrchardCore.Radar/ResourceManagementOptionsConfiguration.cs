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
                .DefineScript("Radar-vue-components")
                .SetDependencies("vuetify-theme")
                .SetUrl("~/StatCan.OrchardCore.Radar/js/radar-vue-components.umd.min.js", "~/StatCan.OrchardCore.Radar/js/radar-vue-components.umd.js")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
