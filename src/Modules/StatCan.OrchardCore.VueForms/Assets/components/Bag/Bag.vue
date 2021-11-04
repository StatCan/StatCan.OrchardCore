<template>
  <div class="pa-1 vue-form-bag-container">
    <div v-show="false">
      <slot name="components"></slot>
      <slot name="validations"></slot>
    </div>

    <slot name="title"></slot>

    <error-message
      :valid="success"
      :error-messages="errorMessages"
    ></error-message>

    <div v-for="(values, i) in internalValues" :key="i">
      <v-row>
        <v-col md="11">
          <div class="mb-3 vue-form-bag-fields-container" v-for="(fieldSet, i) in formComponents" :key="i">
            <component
              :is="fieldSet.validation.name"
              v-bind="fieldSet.validation.props"
              v-slot="{ errors, valid }"
            >
              <component
                v-model="values[valueNames[i]]"
                :is="fieldSet.component.name"
                v-bind="fieldSet.component.props"
                :success="valid"
                :error-messages="errors"
              ></component>
            </component>
          </div>
        </v-col>
        <v-col md="1" class="d-flex align-center pt-1">
          <v-btn
            class="vue-form-bag-remove-button"
            depressed
            v-on:click="removeRow(i)"
            data-cy="vue-form-bag-remove-button"
            >{{ removeButtonLabel }}</v-btn
          >
        </v-col>
      </v-row>
    </div>
    <div class="d-flex justify-center">
      <v-btn
        class="vue-form-bag-add-button"
        depressed
        v-on:click="addRow"
        data-cy="vue-form-bag-add-button"
        >{{ addButtonLabel }}</v-btn
      >
    </div>
  </div>
</template>
<script>
import ErrorMessage from "../common/ErrorMessage.vue";

export default {
  name: "Bag",
  components: { ErrorMessage },
  props: [
    "value",
    "valueNames",
    "success",
    "errorMessages",
    "addButtonLabel",
    "removeButtonLabel"
  ],
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
