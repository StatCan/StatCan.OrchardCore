<template>
 <v-banner v-if="isIE" single-line rounded outlined>
    <v-avatar
      slot="icon"
      size="40"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 256 256"><path d="M224.55,132.63A89,89,0,0,0,213.67,90C243.58,23,182,33,178.33,33a158.1,158.1,0,0,0-36.25,11.78c-41.69-.5-77.94,21.75-89.72,68C74.11,88.23,89.51,78.26,98.58,74.63c-1.91,1.73-3.81,3.54-5.71,5.35a268.73,268.73,0,0,0-36.25,44.41C48.91,138,40.3,149.76,35.77,165.17,11.3,254,88.34,216.83,99.21,210.48a86.24,86.24,0,0,0,39,9,87.59,87.59,0,0,0,82.47-58H174.43c-21.75,34.44-73.41,19-75.22-18.12H224.27a86.06,86.06,0,0,0,.73-10.88ZM208.23,46.54c7.53,5.08,13.6,12.69,3.18,39.88-10-16.32-24.47-29-42.6-35.35,8.07-3.9,28.1-11.78,39.88-4.53Zm-160.4,164c-6.17-6.25-7.25-21.75,6.34-49.84A85.77,85.77,0,0,0,92.23,207c-8.79,4.8-31.72,15.4-44.4,3ZM98.58,120C99.27,100,116.7,83.7,138.45,83.7s39,16.31,39.88,36.25H98.58Z" style="fill:#1ebbee"/><path d="M128,10a118,118,0,0,1,83.44,201.44A118,118,0,0,1,44.56,44.56,117.22,117.22,0,0,1,128,10m0-10A128,128,0,1,0,256,128,128,128,0,0,0,128,0Z" style="fill:#ff1f1f"/><line x1="216.58" y1="39.42" x2="39.42" y2="216.58" style="fill:none;stroke:#ff1f1f;stroke-miterlimit:10;stroke-width:10px"/></svg>    </v-avatar>
    <span v-html="ie">
    </span>
    <template v-slot:actions="{ dismiss }">
      <v-btn
        color="primary"
        text
        @click="dismiss"
      >
        {{ ok }}
      </v-btn>
    </template>
  </v-banner>
</template>

<script>
  export default {
    name: 'ie-banner',
    props: ['ok', 'ie'],
    computed: {
      isIE: function () {
        return navigator.userAgent.indexOf("MSIE ") > -1 || navigator.userAgent.indexOf("Trident/") > -1;
      }
    }
  }
</script>
