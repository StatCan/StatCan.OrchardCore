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
                .SetCdn("https://cdn.jsdelivr.net/npm/vue@2.6.14/dist/vue.min.js", "https://cdn.jsdelivr.net/npm/vue@2.6.14/dist/vue.js")
                .SetCdnIntegrity("sha384-ULpZhk1pvhc/UK5ktA9kwb2guy9ovNSTyxPNHANnA35YjBQgdwI+AhLkixDvdlw4", "sha384-t1tHLsbM7bYMJCXlhr0//00jSs7ZhsAhxgm191xFsyzvieTMCbUWKMhFg9I6ci8q")
                .SetVersion("2.6.14");

            _manifest
                .DefineScript("vee-validate")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vee-validate.full.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vee-validate@3.4.10/dist/vee-validate.full.min.js", "https://cdn.jsdelivr.net/npm/vee-validate@3.4.10/dist/vee-validate.full.js")
                .SetCdnIntegrity("sha384-M1P6JMFPiHTI2eazF774XpnuoOzeBA6w4RlYopvalSwBOCRCReW3Z6gWsg5GqFhv", "sha384-kOXKbG9BGJBxd/hMzJWEM4v1ccD0tLkBwsVRrI9VDmCOHNVSKV9e6xY4lvdi+JET")
                .SetVersion("3.4.10");

            _manifest
                .DefineScript("vuetify")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Scripts/vuetify.min.js", "~/StatCan.OrchardCore.VueForms/Scripts/vuetify.js")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.5.5/dist/vuetify.min.js", "https://cdn.jsdelivr.net/npm/vuetify@2.5.5/dist/vuetify.js")
                .SetCdnIntegrity("sha384-aM4LrL18XipzAqy1wgXNvH/YeI2WJMFBTaRmr5Wlp2SO50ibG9SGWz4TAWEDvN6P", "sha384-bDLMAziYO72UbWT/Bj/Gme/MNJHXCmIK07rcYnwJADxLfvSzKKGf9rJ8v4GrGQOE")
                .SetVersion("2.5.5");

            _manifest
                .DefineStyle("vuetify")
                .SetUrl("~/StatCan.OrchardCore.VueForms/Styles/vuetify.min.css", "~/StatCan.OrchardCore.VueForms/Styles/vuetify.css")
                .SetCdn("https://cdn.jsdelivr.net/npm/vuetify@2.5.5/dist/vuetify.min.css", "https://cdn.jsdelivr.net/npm/vuetify@2.5.5/dist/vuetify.css")
                .SetCdnIntegrity("sha384-Setm/xO0OPABNf1iBVkDBzzyVvvWdN+rXtap1GFWBMHP+4d510lvVMhIk16fz31n", "sha384-K5hhiY0Ik+OrAPfBY9AvS2zlbJwLm8BqjvPQeKHWMUqDX9ojfoZHvw+QJlCR/X78")
                .SetVersion("2.5.5");

            _manifest
                .DefineScript("survey-vue")
                .SetDependencies("vuejs")
                .SetUrl("~/StatCan.OrchardCore.VueForms/survey-vue/survey.vue.min.js", "~/StatCan.OrchardCore.VueForms/survey-vue/survey.vue.js")
                .SetCdn("https://unpkg.com/survey-vue@1.8.52/survey.vue.min.js", "https://unpkg.com/survey-vue@1.8.52/survey.vue.js")
                .SetCdnIntegrity("sha384-zTTLU5YHyWOn9DGD7m4eTnR0uoLdIE0GP2vLtu4TjI3kNwvZbtgyBGtY75uQ0w0R", "sha384-dTyJXjGgOjOM6Wl9DFQ7lN4ncnsqtkivCrj2HCnf/Im1w3s2ZKEyNFCINj2EmVML")
                .SetVersion("1.8.52");

            _manifest
                .DefineStyle("survey-vue")
                .SetUrl("~/StatCan.OrchardCore.VueForms/survey-vue/survey.min.css", "~/StatCan.OrchardCore.VueForms/survey-vue/survey.css")
                .SetCdn("https://unpkg.com/survey-vue@1.8.52/survey.min.css", "https://unpkg.com/survey-vue@1.8.52/survey.css")
                .SetCdnIntegrity("sha384-BC2lcl/MWN+1nHauBJAhb6bdeq7WtirVfhm1QXtDDEvTKkR8lhC9JylQbg59B6u5", "sha384-LKNggg64onQrbdK2rzxAHydgahQPxI28kyocDPpMDMGjnAooQNkslxacSd4vx73L")
                .SetVersion("1.8.52");

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
