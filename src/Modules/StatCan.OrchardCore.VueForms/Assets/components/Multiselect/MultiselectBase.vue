<template>
  <div>
    <multiselect v-bind="$attrs" v-on="$listeners"></multiselect>

    <div class="px-3">
      <error-message
        :valid="success"
        :error-messages="errorMessages"
      ></error-message>
    </div>
  </div>
</template>

<script>
import Multiselect from "vue-multiselect";
import ErrorMessage from "../common/ErrorMessage.vue";

export default {
  name: "MultiselectBase",
  components: { Multiselect, ErrorMessage },
  props: ["success", "errorMessages"]
};
</script>
