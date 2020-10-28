using OrchardCore.ResourceManagement;

namespace StatCan.Themes.HackathonTheme
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineStyle("hackathontheme-styles")
                .SetUrl("~/HackathonTheme/dist/hackathon-theme.css", "~/HackathonTheme/dist/hackathon-theme.css")
                .SetVersion("1.0.0");
            manifest
                .DefineScript("vuejs")
                .SetUrl("~/HackathonTheme/Scripts/vue.min.js", "~/HackathonTheme/Scripts/vue.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.js")
                .SetCdnIntegrity("sha384-cwVe6U8Tq7F/3JIj6xeDzOwuqeChcmRcdYqDGfoYmdAurw7L3f4dFHhEJKfxv96A", "sha384-ma9ivURrHX5VOB4tNq+UiGNkJoANH4EAJmhxd1mmDq0gKOv88wkKZOfRDOpXynwh")
                .SetVersion("2.6.12");
            manifest
                .DefineScript("hackathontheme-scripts")
                .SetDependencies("vuejs")
                .SetUrl("~/HackathonTheme/dist/hackathon-theme.umd.min.js", "~/HackathonTheme/dist/hackathon-theme.umd.js")
                .SetVersion("1.0.0");
        }
    }
}
