<template>
  <div v-show="isActive">
    <slot></slot>
  </div>
</template>

<script>
export default {
  name: "Tab",
  data () {
    return {
      isActive: false,
    };
  }
};
</script>