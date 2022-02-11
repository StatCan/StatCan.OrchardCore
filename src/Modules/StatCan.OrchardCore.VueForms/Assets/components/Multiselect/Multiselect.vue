<template>
  <multiselect-base
    v-model="internalValue"
    v-bind="$attrs"
    :success="success"
    :error-messages="errorMessages"
  >
    <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope"><slot :name="slot" v-bind="scope"/></template>
  </multiselect-base>
</template>

<script>
import MultiselectBase from "./MultiselectBase.vue";

export default {
  name: "Multiselect",
  components: { MultiselectBase },
  props: ["value", "success", "errorMessages"],
  data() {
    return {
      internalValue: null,
      isLoading: false
    };
  },
  created() {
    this.internalValue = this.value;
  },
  watch: {
    internalValue: function(newValue, _) {
      this.$emit("input", newValue);
    }
  }
};
</script>
