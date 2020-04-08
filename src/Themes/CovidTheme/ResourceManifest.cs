using OrchardCore.ResourceManagement;

namespace StatCan.Themes.CovidTheme
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineStyle("CovidTheme-stc-bootstrap")
                .SetUrl("~/CovidTheme/css/covid-bootstrap.min.css", "~/CovidTheme/css/covid-bootstrap.css")
                .SetVersion("1.0.0");
            manifest
                .DefineStyle("CovidTheme-vendor-font-awesome")
                .SetUrl("~/CovidTheme/Vendor/fontawesome-free/css/all.min.css", "~/CovidTheme/Vendor/fontawesome-free/css/all.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/css/all.min.css", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/css/all.css")
                .SetCdnIntegrity("sha384-Bfad6CLCknfcloXFOyFnlgtENryhrpZCe29RTifKEixXQZ38WheV+i/6YWSzkz3V", "sha384-I4s88/QBf6OKVFMRRyjRY9wYdRoEO/PnNLQ1xY+G0OQfntKp8FxRsOg9qjmtbnvL")
                .SetVersion("5.13.0");
            manifest
                .DefineScript("CovidTheme-vendor-font-awesome")
                .SetUrl("~/CovidTheme/Vendor/fontawesome-free/js/all.min.js", "~/CovidTheme/Vendor/fontawesome-free/js/all.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/js/all.min.js", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/js/all.js")
                .SetCdnIntegrity("sha384-ujbKXb9V3HdK7jcWL6kHL1c+2Lj4MR4Gkjl7UtwpSHg/ClpViddK9TI7yU53frPN", "sha384-Z4FE6Znluj29FL1tQBLSSjn1Pr72aJzuelbmGmqSAFTeFd42hQ4WHTc0JC30kbQi")
                .SetVersion("5.13.0");

            manifest
                .DefineScript("CovidTheme-vendor-jQuery")
                .SetUrl("~/CovidTheme/Scripts/jquery.min.js", "~/CovidTheme/Scripts/jquery.js")
                .SetCdn("https://code.jquery.com/jquery-3.4.1.min.js", "https://code.jquery.com/jquery-3.4.1.js")
                .SetCdnIntegrity("sha384-vk5WoKIaW/vJyUAd9n/wmopsmNhiy+L2Z+SBxGYnUkunIxVxAv/UtMOhba/xskxh", "sha384-mlceH9HlqLp7GMKHrj5Ara1+LvdTZVMx4S1U43/NxCvAkzIo8WJ0FE7duLel3wVo")
                .SetVersion("3.4.1");
            manifest
                .DefineScript("CovidTheme-vendor-popper")
                .SetUrl("~/CovidTheme/Scripts/popper.min.js", "~/CovidTheme/Scripts/popper.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js", "https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.js")
                .SetCdnIntegrity("sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo", "sha384-EsqqCR7beeX9mWjsrB8ySgz3pKDhWr3OgqnudRtew5RApIIhEN6/qqiPM99Lk9qM")
                .SetVersion("1.16.0");

            manifest
                .DefineScript("CovidTheme-vendor-bootstrap")
                .SetDependencies("CovidTheme-vendor-jQuery", "CovidTheme-vendor-popper")
                .SetUrl("~/CovidTheme/Scripts/bootstrap.min.js", "~/CovidTheme/Scripts/bootstrap.js")
                .SetCdn("https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js", "https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.js")
                .SetCdnIntegrity("sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6", "sha384-IGTd+U9dY/fgJBXURnLtTaaxga6WSJj46heDWHy/GPu8yyuP3HERqWszUMyWPeWR")
                .SetVersion("4.4.1");
        }
    }
}
