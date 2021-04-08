<template>
  <div class="row">
    <div class="col-2">
      <div class="form-group">
        <div
          class="btn-group-vertical buttons"
          role="group"
          aria-label="Basic example"
        >
        </div>
      </div>
    </div>

    <div class="col-6">
      <h3>Draggable</h3>

      <draggable
        :list="list"
        class="list-group"
        ghost-class="ghost"
        :move="checkMove"
        @start="dragging = true"
        @end="dragging = false"
      >
        <div
          class="list-group-item"
          v-for="element in list"
          :key="element.id"
        >
          {{ element.name }}
        </div>
      </draggable>
    </div>


  </div>
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
  props: ['list'],
  data() {
    return {
      dragging: false
    };
  },
  computed: {
    draggingInfo() {
      return this.dragging ? "under drag" : "";
    }
  },
  methods: {
    checkMove: function(e) {
      window.console.log("Future index: " + e.draggedContext.futureIndex);
    }
  }
};
</script>
<style scoped>
.buttons {
  margin-top: 35px;
}

.ghost {
  opacity: 0.5;
  background: #c8ebfb;
}
</style>
