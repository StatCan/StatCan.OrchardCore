<template>
  <multiselect-base
    v-model="internalValue"
    v-bind="$attrs"
    :success="success"
    :error-messages="errorMessages"
  ></multiselect-base>
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
