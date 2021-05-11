<template>
  <v-row justify="center" name="twoListDraggable">
    
    <v-col cols="4">
      <v-card>
        <v-card-title name="title1">{{ title1 }}</v-card-title>
        <v-card-subtitle name="hint1">{{ hint1 }}</v-card-subtitle>
        <v-divider class="mx-4"></v-divider>
        <draggable
          :list="list1"
          ghost-class="ghost"
          group="listData"
        >
          <v-list-item
            v-for="element in list1"
            :key="element.id"
          >
            {{ element.name }}
          </v-list-item>
        </draggable>
      </v-card>
    </v-col>
    
    <v-col cols="4">
      <v-card >
        <v-card-title name="title2">{{ title2 }}</v-card-title>
        <v-card-subtitle name="hint2">{{ hint2 }}</v-card-subtitle>
        <v-divider class="mx-4"></v-divider>
        <draggable
          :list="list2"
          ghost-class="ghost"
          group="listData"
        >
          <v-list-item
            v-for="element in list2"
            :key="element.id"
          >
            {{ element.name }}
          </v-list-item>
        </draggable>
      </v-card>
    </v-col>

    <v-col cols="8">
      <span id="error" style="color:red">{{ errorMessages[0] }}</span>
    </v-col>

  </v-row>
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
  props: ['list1', 'list2', 'title1', 'title2', 'hint1', 'hint2', 'errorMessages'],
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
