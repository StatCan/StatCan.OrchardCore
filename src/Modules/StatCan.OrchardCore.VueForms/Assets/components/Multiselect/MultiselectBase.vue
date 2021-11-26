<template>
  <div>
    <multiselect v-bind="$attrs" v-on="$listeners">
      <template slot="tag" slot-scope="{ option, remove }">
        <slot name="custom-tag" :option="option" :remove="remove"></slot>
      </template>
      <template slot="option" slot-scope="props">
        <slot name="custom-option" :props="props"></slot>
      </template>
    </multiselect>

    <div class="px-3" v-if="success !== undefined && error-messages !== undefined">
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
