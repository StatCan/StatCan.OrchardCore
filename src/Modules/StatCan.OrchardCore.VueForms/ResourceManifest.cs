using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace StatCan.OrchardCore.VueForms
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest _manifest;

        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineScript("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vue.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vue.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.js")
                .SetCdnIntegrity("sha384-cwVe6U8Tq7F/3JIj6xeDzOwuqeChcmRcdYqDGfoYmdAurw7L3f4dFHhEJKfxv96A", "sha384-ma9ivURrHX5VOB4tNq+UiGNkJoANH4EAJmhxd1mmDq0gKOv88wkKZOfRDOpXynwh")
                .SetVersion("2.6.12");

            _manifest
                .DefineScript("vee-validate")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vee-validate@3.4.4/dist/vee-validate.full.min.js", "https://cdn.jsdelivr.net/npm/vee-validate@3.4.4/dist/vee-validate.full.js")
                .SetCdnIntegrity("sha384-tn0idebO6GZEY8mGV35ylsBfl51vwSVL055B3Y20gE0Q6p+UqX99nUWu9k4pVAhv", "sha384-zxq+aE29vXErUV36F/msIf+ZrGzTn0wKRxtAyz/vT0Ga2XSM86KfyK+qGJNnjXXZ")
                .SetVersion("3.4.4");

            _manifest
                .DefineScript("vuetify")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vuetify.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vuetify.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.5.0/dist/vuetify.min.js", "https://cdn.jsdelivr.net/npm/vuetify@2.5.0/dist/vuetify.js")
                .SetCdnIntegrity("sha384-ZTKEVv47kt3G9r7yBdHQcgC+fNXqtLkdFR16db/FlOP6QV31arRRRCd2SWZtuRPM", "sha384-AtuJi0H+TrtLABhGFZqBHLt7T6TdRaqby6PfhzMb+Uo+v5ohuZQtA3QO/1nY+bMQ")
                .SetVersion("2.5.0");

            _manifest
                .DefineStyle("vuetify")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vuetify.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vuetify.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.5.0/dist/vuetify.min.css", "https://cdn.jsdelivr.net/npm/vuetify@2.5.0/dist/vuetify.css")
                .SetCdnIntegrity("sha384-dWWOZoIT5RMdoDxZhVcOa85D1WR4mwqLg5i1GnXkCMLfoly2SgbhIIhzMajvALfc", "sha384-3YRJqq9ESY6mL98bzdhaP0r0BauILz3QGLFJvhGW485VUWGC/C1BFoPM8b48SHzv")
                .SetVersion("2.5.0");

            _manifest
                .DefineStyle("vue-forms")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vue-forms.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vue-forms.css")
                .SetVersion("1.0.0");

            _manifest
               .DefineScript("vue-forms")
               .SetDependencies("jQuery", "vuejs", "vee-validate")
               .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js", "~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js")
               .SetVersion("1.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
