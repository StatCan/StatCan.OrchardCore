import 'core-js/stable'
import 'regenerator-runtime/runtime'

import Vue from 'vue'
import vuetify from './plugins/vuetify'
import IEBanner from './components/IEBanner.vue'

import './styles.scss'
import './sass/github-markdown.scss'

Vue.component('ie-banner', IEBanner);

document.addEventListener("DOMContentLoaded", function() {
  new Vue({
    vuetify,
    data: () => ({ drawer: null }),
  }).$mount('#page-top')
});
