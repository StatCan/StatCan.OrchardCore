<template>
  <div class="pa-1">
    <div v-show="false">
      <slot></slot>
    </div>
    <div class="text-h5 mb-5">
      {{ title }}
    </div>
    <div
      class="d-flex justify-center"
      v-for="(values, i) in internalValues"
      :key="i"
    >
      <div class="m-auto" v-for="(component, i) in formComponents" :key="i">
        <component
          class="mr-5"
          v-model="values[valueNames[i]]"
          :is="component.name"
          v-bind="component.props"
        ></component>
      </div>
      <div class="mt-3">
        <v-btn depressed v-on:click="removeRow(i)">-</v-btn>
      </div>
    </div>
    <div class="d-flex justify-center">
      <v-btn depressed v-on:click="addRow">+</v-btn>
    </div>
  </div>
</template>
<script>
export default {
  name: "Bag",
  props: ["value", "valueNames", "title"],
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
