<template>
  <div>
    <multiselect v-bind="$attrs" v-on="$listeners"></multiselect>
    {{ errorMessages }}
  </div>
</template>

<script>
import Multiselect from "vue-multiselect";

export default {
  name: "MultiselectBase",
  components: { Multiselect },
  data() {
    return {};
  },
  created() {
    console.log(this);
  }
};
</script>
