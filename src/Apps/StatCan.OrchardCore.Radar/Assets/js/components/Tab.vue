<template>
  <div v-show="isActive">
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "Tab",
  props: ["title", "icon", "count"],
  data () {
    return {
      isActive: false,
    };
  }
};
</script>