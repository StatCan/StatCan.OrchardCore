<template>
  <div>
    <div v-for="(component, index) in components" :key="index">
      <component
        v-model="internalValues[valueNames[i]]"
        :is="component.name"
        v-bind="component.props"
      ></component>
    </div>
  </div>
</template>
<script>
export default {
  name: "BagRow",
  props: ["values", "components", "valueNames"],
  data() {
    return {
      internalValues: {}
    };
  },
  created() {
    this.internalValues = this.values;
  },
  watch: {
    internalValues: {
      handler: function(newValue, _) {},
      deep: true
    }
  }
};
</script>
