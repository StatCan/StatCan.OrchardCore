import Vue from 'vue'
import vuetify from './plugins/vuetify'
import IEBanner from './components/IEBanner.vue'
import Draggable from './components/Draggable.vue'
import TwoListsDraggable from './components/TwoListsDraggable.vue'

import "@fontsource/roboto/100.css"
import "@fontsource/roboto/300.css"
import "@fontsource/roboto/400.css"
import "@fontsource/roboto/500.css"
import "@fontsource/roboto/700.css"
import "@fontsource/roboto/900.css"
import "@mdi/font/css/materialdesignicons.css" 

import './sass/styles.scss' 

Vue.component('ie-banner', IEBanner);
Vue.component('sort-draggable', Draggable);
Vue.component('two-lists-draggable', TwoListsDraggable);

function higlightMenu() {
   // highlight the current menu tree in the drawer if present.
   const currentUrl = window.location.pathname;
   const menus = document.querySelectorAll('[data-menu="item"]');
   //console.log("menu list: ", menus);
   for(let i=0; i<menus.length; i++) {
     const menu = menus[i];
    // console.log("menu: ", menu);
     const href = menu.getAttribute('href');
     if(href)
     {
       if(currentUrl == href)
       {
         // console.log("match");
         menu.setAttribute("input-value", "true");
         // iterate through all parents and set the active class of all groups
         let parent = menu.parentNode;
         while (parent && typeof parent.hasAttribute === "function") {
           // console.log("parent", parent);
           if(parent.hasAttribute("data-menu")) {
             if(parent.dataset.menu == 'group')
             {
               // console.log("parentMatch");
               parent.setAttribute("value", "true");
             }
           }
           parent = parent.parentNode;
         }
         //break out of for loop, we are done
         break;
       }
     }
   }
}

higlightMenu();

document.addEventListener("DOMContentLoaded", function() {
  // console.log("loaded");
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
