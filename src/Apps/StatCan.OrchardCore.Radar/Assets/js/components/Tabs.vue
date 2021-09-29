<template>
  <div>
    <div class="tabs mb-10">
      <button
        v-for="(tab, index) in tabs"
        :key="index"
        @click="selectTab(index)"
        :class="tabButtonClasses(index)"
      >
        <div class="tab-header-container">
          <v-icon class="mr-2">
            {{ tab.icon }}
          </v-icon>
          <div>{{ tab.title }}</div>
        </div>
      </button>
    </div>
    <slot name="tab"></slot>
  </div>
</template>

<script>
export default {
  name: "Tabs",
  data() {
    return {
      selectedIndex: 0,
      tabs: []
    };
  },
  mounted() {
    this.tabs = this.$slots.tab.map(node => {
      return node.componentInstance;
    });
    this.selectTab(0);
  },
  methods: {
    selectTab(i) {
      this.selectedIndex = i;
      this.tabs.forEach((tab, index) => {
        tab.isActive = index === i;
      });
    },
    tabButtonClasses(index) {
      return `${
        index === this.selectedIndex ? "selected" : "unselected"
      } tab-button`;
    }
  }
};
</script>
