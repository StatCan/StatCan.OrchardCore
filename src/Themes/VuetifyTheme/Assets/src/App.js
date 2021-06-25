import Vue from 'vue'
import vuetify from './plugins/vuetify'
import IEBanner from './components/IEBanner.vue'

import "@fontsource/roboto/100.css"
import "@fontsource/roboto/300.css"
import "@fontsource/roboto/400.css"
import "@fontsource/roboto/500.css"
import "@fontsource/roboto/700.css"
import "@fontsource/roboto/900.css"
import "@mdi/font/css/materialdesignicons.css"

import './styles.scss'
import './sass/github-markdown.scss'

Vue.component('ie-banner', IEBanner);

// Sets the cookie based on the current html lang. This is to avoid potential issues when landing on a french page.
document.cookie = '.AspNetCore.Culture=c%3D'+document.documentElement.lang+'%7Cuic%3D'+document.documentElement.lang +';path=/';
document.addEventListener("DOMContentLoaded", function() {
  new Vue({
    vuetify,
    data: () => ({ drawer: null }),
    methods: {
      darkMode() {
        this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
        localStorage.setItem("VuetifyThemeDarkMode", this.$vuetify.theme.dark);
      }
    },
  }).$mount('#vuetify-theme-app')
});
