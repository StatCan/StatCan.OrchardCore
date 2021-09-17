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
                .DefineStyle("Radar Stylesheet")
                .SetUrl("~/StatCan.OrchardCore.Radar/css/index.css", "~/StatCan.OrchardCore.Radar/css/index.css")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
