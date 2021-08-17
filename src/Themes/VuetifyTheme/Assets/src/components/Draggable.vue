<template>
      <v-card name="singleListDraggable">
        <v-card-title name="title">{{ title }}</v-card-title>
        <v-card-subtitle name="hint">{{ hint }}</v-card-subtitle>
        <v-divider class="mx-4"></v-divider>
        <draggable
          :list="list"
          ghost-class="ghost"
          @start="dragging = true"
          @end="dragging = false"
        >
          <v-list-item
            v-for="element in list"
            :key="element.id"
          >
            {{ element.name }}
          </v-list-item>
        </draggable>
      </v-card>
</template>

<script>
import draggable from "vuedraggable";
export default {
  name: "simple",
  display: "Simple",
  order: 0,
  components: {
    draggable
  },
  props: ['list', 'title', 'hint'],
  data() {
    return {
      dragging: false
    };
  }
};
</script>
<style scoped>
.ghost {
  opacity: 0.5;
  background: #c8ebfb;
}
</style>
