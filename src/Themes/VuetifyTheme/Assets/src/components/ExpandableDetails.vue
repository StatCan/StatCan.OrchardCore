<template>
  <div id="clickbox" v-click-outside="onOutsideClick">
    <div :style="{ backgroundColor: computedColor }">
      <div v-on:click="show = !show" style="display: inline-block">
        <slot name="default"></slot>
      </div>
    </div>
    <div v-show="show">
      <slot name="expanded"></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: "expandable-details",
  props: {
    defaultColor: String,
    expandedColor: String,
  },
  data: function () {
    return { show: false };
  },
  computed: {
    computedColor: function () {
      let dColor = this.defaultColor ? this.defaultColor : "white";
      return this.show ? this.expandedColor : dColor;
    },
  },
  methods: {
    onOutsideClick() {
      this.show = false;
    }
  }
};
</script>