using OrchardCore.ResourceManagement;

namespace StatCan.Themes.PortalTheme
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineStyle("PortalTheme-stc-bootstrap")
                .SetUrl("~/PortalTheme/css/portal-bootstrap.min.css", "~/PortalTheme/css/portal-bootstrap.css")
                .SetVersion("1.0.0");
            manifest
                .DefineStyle("font-awesome")
                .SetUrl("~/PortalTheme/Vendor/fontawesome-free/css/all.min.css", "~/PortalTheme/Vendor/fontawesome-free/css/all.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/css/all.min.css", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/css/all.css")
                .SetCdnIntegrity("sha384-Bfad6CLCknfcloXFOyFnlgtENryhrpZCe29RTifKEixXQZ38WheV+i/6YWSzkz3V", "sha384-I4s88/QBf6OKVFMRRyjRY9wYdRoEO/PnNLQ1xY+G0OQfntKp8FxRsOg9qjmtbnvL")
                .SetVersion("5.13.0");
            manifest
                .DefineScript("font-awesome")
                .SetUrl("~/PortalTheme/Vendor/fontawesome-free/js/all.min.js", "~/PortalTheme/Vendor/fontawesome-free/js/all.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/js/all.min.js", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.13.0/js/all.js")
                .SetCdnIntegrity("sha384-ujbKXb9V3HdK7jcWL6kHL1c+2Lj4MR4Gkjl7UtwpSHg/ClpViddK9TI7yU53frPN", "sha384-Z4FE6Znluj29FL1tQBLSSjn1Pr72aJzuelbmGmqSAFTeFd42hQ4WHTc0JC30kbQi")
                .SetVersion("5.13.0");

            manifest
                .DefineScript("jQuery")
                .SetUrl("~/PortalTheme/Scripts/jquery.min.js", "~/PortalTheme/Scripts/jquery.js")
              .SetCdn("https://code.jquery.com/jquery-3.4.1.min.js", "https://code.jquery.com/jquery-3.4.1.js")
              .SetCdnIntegrity("sha384-vk5WoKIaW/vJyUAd9n/wmopsmNhiy+L2Z+SBxGYnUkunIxVxAv/UtMOhba/xskxh", "sha384-mlceH9HlqLp7GMKHrj5Ara1+LvdTZVMx4S1U43/NxCvAkzIo8WJ0FE7duLel3wVo")
              .SetVersion("3.4.1");
            manifest
                .DefineScript("popper")
                .SetUrl("~/PortalTheme/Scripts/popper.min.js", "~/PortalTheme/Scripts/popper.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js", "https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.js")
                .SetCdnIntegrity("sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN", "sha384-cpSm/ilDFOWiMuF2bj03ZzJinb48NO9IGCXcYDtUzdP5y64Ober65chnoOj1XFoA")
                .SetVersion("1.16.1");

            manifest
                .DefineScript("bootstrap")
                .SetDependencies("jQuery", "popper")
                .SetUrl("~/PortalTheme/Scripts/bootstrap.min.js", "~/PortalTheme/Scripts/bootstrap.js")
                .SetCdn("https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js", "https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.js")
                .SetCdnIntegrity("sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6", "sha384-IGTd+U9dY/fgJBXURnLtTaaxga6WSJj46heDWHy/GPu8yyuP3HERqWszUMyWPeWR")
                .SetVersion("4.4.1");
        }
    }
}
