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
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.11/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.11/dist/vue.js")
                .SetCdnIntegrity("sha384-OZmxTjkv7EQo5XDMPAmIkkvywVeXw59YyYh6zq8UKfkbor13jS+5p8qMTBSA1q+F", "sha384-+jvb+jCJ37FkNjPyYLI3KJzQeD8pPFXUra3B/QJFqQ3txYrUPIP1eOfxK4h3cKZP")
                .SetVersion("2.6.11");
            manifest
                .DefineScript("vee-validate")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vee-validate@3.3.11/dist/vee-validate.full.min.js", "https://cdn.jsdelivr.net/npm/vee-validate@3.3.11/dist/vee-validate.full.js")
                .SetCdnIntegrity("sha384-9WkogwiBkp0ktjKxFH+9QKX2cfiWI6UvcxOD5MExVaCnK8mqJkxubbsaeFlebNYS", "sha384-pit7Y0lTrHtTYEQDF9w3oV0fIjfPYV1r2Z5UCvNSaratRyTzwq4e+IfZXLjxM80S")
                .SetVersion("3.3.11");
            manifest
                .DefineScript("vuetify")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vuetify.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vuetify.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.3.10/dist/vuetify.min.js", "https://cdn.jsdelivr.net/npm/vuetify@2.3.10/dist/vuetify.js")
                .SetCdnIntegrity("sha384-HPkhWtgo1Yqb2jzqZJ91xDQ+c+Rlj6ANjQN5srMDB5RUsDI2Ff/WKBrvEbv3id+c", "sha384-fonCq3wQKhz2JW3eDMQRRpuRAFhwaqa6dt6XrsCIXBtBo3bMbAdDLtrAOxeVFMId")
                .SetVersion("2.3.10");
            manifest
                .DefineStyle("vuetify")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vuetify.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vuetify.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.3.10/dist/vuetify.min.css", "https://cdn.jsdelivr.net/npm/vuetify@2.3.10/dist/vuetify.css")
                .SetCdnIntegrity("sha384-lZ/7sTmigDtUbdDwbW5+uR0ev6DiONA348CAcJbzaKaKSife4dMk1+77FDwBjd52", "sha384-UH/a0cVop3B4MebOcqXSKp3HknBnXCo1m7wRkQCyqNw0s9xUORnEdydOCf6hlZhS")
                .SetVersion("2.3.10");

            manifest
                .DefineStyle("vue-forms")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vue-forms.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vue-forms.css")
                .SetVersion("1.0.0");
            manifest
               .DefineScript("vue-forms")
               .SetDependencies("vuejs", "vuetify", "vee-validate")
               .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js", "~/StatCan.OrchardCore.VueForms/Scripts/vue-forms.js")
               .SetVersion("1.0.0");
        }
    }
}
