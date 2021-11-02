<template>
  <div class="pa-1">
    <div v-show="false">
      <slot name="components"></slot>
      <slot name="validations"></slot>
    </div>
    <div class="text-h5 mb-5">
      {{ title }}
    </div>

    <!-- Uses the same transition from: -->
    <!-- https://github.com/vuetifyjs/vuetify/blob/aa817a453dcd625c0aec1e2305907f357666258d/packages/vuetify/src/components/VMessages/VMessages.ts#L32 -->
    <transition name="message-transition">
      <div v-if="!valid" class="error--text v-messages">
        <div v-for="(message, i) in errorMessages" :key="i">
          {{ message }}
        </div>
      </div>
    </transition>

    <div
      class="d-flex justify-center"
      v-for="(values, i) in internalValues"
      :key="i"
    >
      <div class="m-auto" v-for="(fieldSet, i) in formComponents" :key="i">
        <component
          :is="fieldSet.validation.name"
          v-bind="fieldSet.validation.props"
          v-slot="{ errors, valid }"
        >
          <component
            class="mr-5"
            v-model="values[valueNames[i]]"
            :is="fieldSet.component.name"
            v-bind="fieldSet.component.props"
            :success="valid"
            :error-messages="errors"
          ></component>
        </component>
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
  props: ["value", "valueNames", "title", "valid", "errorMessages"],
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
    const components = this.getComponentsFromSlot("components");
    const validations = this.getComponentsFromSlot("validations");

    // Each field must have a validator
    if (components.length !== validations.length) {
      throw new Error("Each field must have a validator");
    }

    for (let i = 0; i < components.length; i++) {
      const obj = {};

      obj.component = { ...components[i] };
      obj.validation = { ...validations[i] };

      this.formComponents.push(obj);
    }
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
    },
    // Helper methods
    getComponentsFromSlot(slotName) {
      return this.$slots[slotName]
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
    }
  },
  watch: {
    internalValues: {
      handler: function(newValue, _) {
        this.$emit("input", newValue);
      },
      deep: true
    }
  }
};
</script>
