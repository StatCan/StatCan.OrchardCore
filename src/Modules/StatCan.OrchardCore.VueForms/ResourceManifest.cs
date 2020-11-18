using OrchardCore.ResourceManagement;

namespace StatCan.OrchardCore.VueForms
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(IResourceManifestBuilder builder)
        {
            var manifest = builder.Add();

            manifest
                .DefineScript("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vue.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vue.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.12/dist/vue.js")
                .SetCdnIntegrity("sha384-cwVe6U8Tq7F/3JIj6xeDzOwuqeChcmRcdYqDGfoYmdAurw7L3f4dFHhEJKfxv96A", "sha384-ma9ivURrHX5VOB4tNq+UiGNkJoANH4EAJmhxd1mmDq0gKOv88wkKZOfRDOpXynwh")
                .SetVersion("2.6.12");

            manifest
                .DefineScript("vee-validate")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vee-validate@3.4.4/dist/vee-validate.full.min.js", "https://cdn.jsdelivr.net/npm/vee-validate@3.4.4/dist/vee-validate.full.js")
                .SetCdnIntegrity("sha384-tn0idebO6GZEY8mGV35ylsBfl51vwSVL055B3Y20gE0Q6p+UqX99nUWu9k4pVAhv", "sha384-zxq+aE29vXErUV36F/msIf+ZrGzTn0wKRxtAyz/vT0Ga2XSM86KfyK+qGJNnjXXZ")
                .SetVersion("3.4.4");

            manifest
                .DefineScript("vuetify")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vuetify.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vuetify.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.3.16/dist/vuetify.min.js", "https://cdn.jsdelivr.net/npm/vuetify@2.3.16/dist/vuetify.js")
                .SetCdnIntegrity("sha384-oWs61kcvUHUcvKDasUC1/NOLdo+TgLyQ6naS89TiEz+0/rM1YvEEwSQWQdfvgi8X", "sha384-KHkNwxwHw/d8M1bpUtFKyKgIbSGPUiZZRkN2MIoY/YTkFulpgXUGDD58NfaaMZm/")
                .SetVersion("2.3.16");

            manifest
                .DefineStyle("vuetify")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vuetify.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vuetify.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.3.16/dist/vuetify.min.css", "https://cdn.jsdelivr.net/npm/vuetify@2.3.16/dist/vuetify.css")
                .SetCdnIntegrity("sha384-C0yQzReUZVYGwxgpET43LgD1tn0L3Hv2/HSL6RTcL1HLPCPHBf7QHY+Y+TPBGZxp", "sha384-aBPEGa60gC4/krAhSCZzRWSdXY4rLG2KWrugfUKftJNVdXcncArtH4LFesz5LaxF")
                .SetVersion("2.3.16");

            manifest
                .DefineStyle("vue-forms")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vue-forms.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vue-forms.css")
                .SetVersion("1.0.0");

            manifest
               .DefineScript("vue-forms")
               .SetDependencies("jQuery", "vuejs", "vee-validate")
               .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js", "~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js")
               .SetVersion("1.0.0");
        }
    }
}
