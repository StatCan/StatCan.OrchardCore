<template>
  <div>
    <div v-show="false">
      <slot></slot>
    </div>
    <button v-on:click="addRow">+</button>
    <div v-for="(values, i) in internalValues" :key="i">
      <button v-on:click="removeRow(i)">-</button>
      <div v-for="(component, i) in formComponents" :key="i">
        <component
          v-model="values[valueNames[i]]"
          :is="component.name"
          v-bind="component.props"
        ></component>
      </div>
    </div>
  </div>
</template>
<script>
import BagRow from "./BagRow.vue";

export default {
  name: "Bag",
  components: { BagRow },
  props: ["value", "valueNames"],
  data() {
    return {
      formComponents: [],
      internalValues: []
    };
  },
  created() {
    this.internalValues = this.value;
  },
  mounted() {
    // Create the "component prototypes"
    this.formComponents = this.$slots.default
      .filter(component => component.componentOptions)
      .map(component => {
        return {
          name: component.componentOptions.tag,
          props: {
            ...component.data.attrs,
            ...component.componentOptions.propsData
          }
        };
      });
  },
  methods: {
    addRow() {
      this.internalValues.push(
        this.valueNames.reduce((acc, curr) => ((acc[curr] = ""), acc), {})
      );
    },
    removeRow(rowIndex) {
      this.internalValues = this.internalValues.filter(
        (_, i) => i !== rowIndex
      );
    }
  },
  watch: {
    internalValues: {
      handler: function(newValue, _) {
        console.log(newValue);
        this.$emit("input", newValue);
      },
      deep: true
    }
  }
};
</script>
