<template>
  <div class="global-search-container">
    <v-icon color="white" class="mr-5">
      mdi-magnify
    </v-icon>
    <input
      v-model.trim="searchText"
      @input="isTyping = true"
      class="global-search"
      type="text"
      :placeholder="placeholder"
    />
    <div class="dropdown-list" ref="dropdownResult" v-show="showDropdown">
      <div v-show="projects.length > 0">
        <div class="result-caption">
          <v-icon class="mr-3 icon-primary">mdi-flask-outline</v-icon>
          <div>{{ projectTitle }}</div>
        </div>
        <div
          v-for="item in projects"
          :key="item.name"
          class="dropdown-item"
        >
          <a class="black--text" :href="item.AutoroutePart.Path">{{ item.DisplayText }}</a>
        </div>
      </div>

      <div v-show="communities.length > 0">
        <div class="result-caption">
          <v-icon class="mr-3 icon-primary">mdi-account-multiple</v-icon>
          <div>{{ communityTitle }}</div>
        </div>
        <div
          v-for="item in communities"
          :key="item.name"
          class="dropdown-item"
        >
          <a class="black--text" :href="item.AutoroutePart.Path">{{ item.DisplayText }}</a>
        </div>
      </div>

      <div v-show="events.length > 0">
        <div class="result-caption">
          <v-icon class="mr-3 icon-primary">mdi-calendar</v-icon>
          <div>{{ eventTitle }}</div>
        </div>
        <div v-for="item in events" :key="item.name" class="dropdown-item">
          <a class="black--text" :href="item.AutoroutePart.Path">{{ item.DisplayText }}</a>
        </div>
      </div>

      <div v-show="proposals.length > 0">
        <div class="result-caption">
          <v-icon class="mr-3 icon-primary">mdi-alert-circle-outline</v-icon>
          <div>{{ communityTitle }}</div>
        </div>
        <div
          v-for="item in proposals"
          :key="item.name"
          class="dropdown-item"
        >
          <a class="black--text" :href="item.AutoroutePart.Path">{{ item.DisplayText }}</a>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import debounce from "lodash/debounce";

export default {
  props: [
    "apiUrl",
    "placeholder",
    "projectTitle",
    "eventTitle",
    "communityTitle",
    "proposalTitle"
  ],
  data: function() {
    return {
      searchText: "",
      isTyping: false,
      projects: [],
      events: [],
      proposals: [],
      communities: [],
      showDropdown: false
    };
  },
  watch: {
    searchText: debounce(function() {
      this.isTyping = false;
    }, 1000),
    isTyping: function(value) {
      if (!value) {
        this.getResult(this.searchText);
      }
    }
  },
  created() {
    document.addEventListener("click", this.documentClick);
  },
  destroyed() {
    document.removeEventListener("click", this.documentClick);
  },
  methods: {
    getResult(searchText) {
      fetch(this.apiUrl + `?searchText=${searchText}`, {
        method: "GET"
      })
        .then(response => response.json())
        .then(data => {
          if (data.length > 0) {
            this.showDropdown = true;
          } else {
            this.showDropdown = false;
          }

          this.communities = data.filter(
            content => content.ContentType === "Community"
          );
          this.projects = data.filter(
            content => content.ContentType === "Project"
          );
          this.events = data.filter(content => content.ContentType === "Event");
          this.proposals = data.filter(
            content => content.ContentType === "Proposal"
          );
        });
    },
    documentClick(e) {
      let el = this.$refs.dropdownResult;
      let target = e.target;
      if (el !== target && !el.contains(target)) {
        this.showDropdown = false;
      }
    },
    isListEmpty(list) {
      return list.length === 0;
    }
  }
};
</script>
