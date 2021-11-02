<template>
  <!-- Uses the same transition from: -->
  <!-- https://github.com/vuetifyjs/vuetify/blob/aa817a453dcd625c0aec1e2305907f357666258d/packages/vuetify/src/components/VMessages/VMessages.ts#L32 -->
  <transition name="message-transition">
    <div v-if="!valid" class="error--text v-messages">
      <div v-for="(message, i) in errorMessages" :key="i">
        {{ message }}
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: "ErrorMessage",
  props: ["valid", "errorMessages"]
};
</script>
