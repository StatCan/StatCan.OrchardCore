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

import './sass/styles.scss' 

Vue.component('ie-banner', IEBanner);
 
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
