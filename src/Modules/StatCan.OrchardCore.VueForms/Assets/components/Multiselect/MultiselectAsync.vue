<template>
  <multiselect-base
    v-model="internalValue"
    :options="options"
    :loading="isLoading"
    @search-change="asyncFind"
    v-bind="$attrs"
    :success="success"
    :error-messages="errorMessages"
  >
    <template v-for="(_, slot) of $scopedSlots" v-slot:[slot]="scope"><slot :name="slot" v-bind="scope"/></template>
  </multiselect-base>
</template>

<script>
import MultiselectBase from "./MultiselectBase.vue";
import debounce from "lodash/debounce";

export default {
  name: "MultiselectAsync",
  components: { MultiselectBase },
  props: ["value", "api", "success", "errorMessages"],
  data() {
    return {
      internalValue: null,
      options: [],
      isLoading: false
    };
  },
  created() {
    this.internalValue = this.value;
  },
  methods: {
    asyncFind: debounce(function(query) {
      if (query.length > 0) {
        this.isLoading = true;
        fetch(this.api + query, {
          method: "GET",
          credentials: "include"
        })
          .then(res => res.json())
          .then(data => {
            this.options = data;
            this.isLoading = false;
          });
      }
    }, 1000)
  },
  watch: {
    internalValue: function(newValue, _) {
      this.$emit("input", newValue);
    }
  }
};
</script>
