using OrchardCore.ResourceManagement;

namespace OrchardCore.Themes.TheHeadlessTheme
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineStyle("TheHeadlessTheme-bootstrap-oc")
                .SetUrl("~/TheHeadlessTheme/css/bootstrap-oc.min.css", "~/TheHeadlessTheme/css/bootstrap-oc.css")
                .SetVersion("1.0.0");
				
        }
    }
}
