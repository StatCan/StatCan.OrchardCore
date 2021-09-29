using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace StatCan.Themes.BootstrapTheme
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static readonly ResourceManifest _manifest;
        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineStyle("BootstrapTheme")
                .SetUrl("~/BootstrapTheme/css/bootstrap-theme.min.css", "~/BootstrapTheme/css/bootstrap-theme.css")
                .SetVersion("1.0.0");
            _manifest
                .DefineStyle("font-awesome")
                .SetUrl("~/BootstrapTheme/Vendor/fontawesome-free/css/all.min.css", "~/BootstrapTheme/Vendor/fontawesome-free/css/all.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/css/all.min.css", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/css/all.css")
                .SetCdnIntegrity("sha384-SZXxX4whJ79/gErwcOYf+zWLeJdY/qpuqC4cAa9rOGUstPomtqpuNWT9wdPEn2fk", "sha384-vWCAomWWdFMlaYCU/mSVGlDyfz8wsJ3jTF1DT6ZvnwoQBnvB4mzjRVWbD8VUAm2U")
                .SetVersion("5.15.3");
            _manifest
                .DefineScript("font-awesome")
                .SetUrl("~/BootstrapTheme/Vendor/fontawesome-free/js/all.min.js", "~/BootstrapTheme/Vendor/fontawesome-free/js/all.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/js/all.min.js", "https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.3/js/all.js")
                .SetCdnIntegrity("sha384-haqrlim99xjfMxRP6EWtafs0sB1WKcMdynwZleuUSwJR0mDeRYbhtY+KPMr+JL6f", "sha384-E+5pdwyxZrKKKwqEbkLV9ObY62/A7RNpE4L95tvjbBYRTy9XfPWqyCdmlgt8egj7")
                .SetVersion("5.15.3");
            _manifest
                .DefineStyle("mdi-icons")
                .SetUrl("~/BootstrapTheme/Vendor/mdi-icons/css/materialdesignicons.min.css", "~/BootstrapTheme/Vendor/mdi-icons/css/materialdesignicons.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/@mdi/font@5.9.55/css/materialdesignicons.min.css", "https://cdn.jsdelivr.net/npm/@mdi/font@5.9.55/css/materialdesignicons.css")
                .SetCdnIntegrity("sha384-kXCE8Tlo8eDlJdLjfNasjLDUTPO56LHJRXxPOIRAgeAjnT08vds9p4SF9Q4tuTD+", "sha384-StrlZjgUXZy66I1Rq7FAeJiI66r+MOfZLA1KA7dhlOY3Ef5RHv1I2JZhUnEkaw3S")
                .SetVersion("5.9.55");
            _manifest
                .DefineScript("jQuery")
                .SetUrl("~/BootstrapTheme/Scripts/jquery.min.js", "~/BootstrapTheme/Scripts/jquery.js")
                .SetCdn("https://code.jquery.com/jquery-3.6.0.min.js", "https://code.jquery.com/jquery-3.6.0.js")
                .SetCdnIntegrity("sha384-vtXRMe3mGCbOeY7l30aIg8H9p3GdeSe4IFlP6G8JMa7o7lXvnz3GFKzPxzJdPfGK", "sha384-S58meLBGKxIiQmJ/pJ8ilvFUcGcqgla+mWH9EEKGm6i6rKxSTA2kpXJQJ8n7XK4w")
                .SetVersion("3.6.0");
            _manifest
                .DefineScript("popper")
                .SetUrl("~/BootstrapTheme/Scripts/popper.min.js", "~/BootstrapTheme/Scripts/popper.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js", "https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.js")
                .SetCdnIntegrity("sha384-9/reFTGAW83EW2RDu2S0VKaIzap3H66lZH81PoYlFhbGU+6BZp6G7niu735Sk7lN", "sha384-cpSm/ilDFOWiMuF2bj03ZzJinb48NO9IGCXcYDtUzdP5y64Ober65chnoOj1XFoA")
                .SetVersion("1.16.1");

            _manifest
                .DefineScript("bootstrap")
                .SetDependencies("jQuery", "popper")
                .SetUrl("~/BootstrapTheme/Scripts/bootstrap.min.js", "~/BootstrapTheme/Scripts/bootstrap.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.min.js", "https://cdn.jsdelivr.net/npm/bootstrap@4.6.0/dist/js/bootstrap.min.js")
                .SetCdnIntegrity("sha384-+YQ4JLhjyBLPDQt//I+STsc9iw4uQqACwlvpslubQzn4u2UU2UFM80nGisd026JF", "sha384-AOHPfOD4WCwCMAGJIzdIL1mo+l1zLNufRq4DA9jDcW1Eh1T3TeQoRaq9jJq0oAR0")
                .SetVersion("4.6.0");
            _manifest
                .DefineScript("details-polyfill")
                .SetUrl("~/BootstrapTheme/Scripts/details-element-polyfill.min.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/details-element-polyfill@2.4.0/dist/details-element-polyfill.min.js")
                .SetCdnIntegrity("sha384-pqTuXkYAErLQ9cr0gZChyC6CxAP3nFWj2wFOwcI/C28oy5UKaMfPuKVeuS9wn3MZ")
                .SetVersion("2.4.0");
        }
        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
