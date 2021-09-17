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
                .DefineStyle("landing-page-style")
                .SetUrl("~/StatCan.OrchardCore.Radar/css/landing-page.css", "~/StatCan.OrchardCore.Radar/css/landing-page.css")
                .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
