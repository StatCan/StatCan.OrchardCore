<template>
  <div v-show="isActive">
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "Tab",
  props: ["title", "icon"],
  data () {
    return {
      isActive: false,
    };
  }
};
</script>