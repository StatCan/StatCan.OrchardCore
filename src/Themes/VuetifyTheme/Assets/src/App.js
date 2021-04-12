import Vue from 'vue'
import vuetify from './plugins/vuetify'

import './styles.scss'
import './sass/github-markdown.scss'

// Sets the cookie based on the current html lang. This is to avoid potential issues when landing on a french page.
document.cookie = '.AspNetCore.Culture=c%3D'+document.documentElement.lang+'%7Cuic%3D'+document.documentElement.lang +';path=/';

document.addEventListener("DOMContentLoaded", function() {
  new Vue({
    vuetify,
    data: () => ({ drawer: null }),
  }).$mount('#page-top')
});
