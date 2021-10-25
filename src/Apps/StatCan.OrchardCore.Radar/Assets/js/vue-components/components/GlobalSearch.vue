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
      <template v-for="(section, key) in sections">
        <div :key="section.title.Value" v-show="entities[key].length > 0">
          <div class="result-caption">
            <v-icon class="mr-3 icon-primary">{{ section.icon }}</v-icon>
            <div>{{ section.title.Value }}</div>
          </div>
          <div
            v-for="item in entities[key]"
            :key="item.name"
            class="dropdown-item"
          >
            <a class="black--text" :href="item.AutoroutePart.Path">{{
              item.DisplayText
            }}</a>
          </div>
        </div>
      </template>
    </div>
  </div>
</template>

<script>
import debounce from "lodash/debounce";

export default {
  props: ["apiUrl", "placeholder", "titles"],
  data: function() {
    return {
      searchText: "",
      isTyping: false,
      showDropdown: false,
      sections: {},
      entities: {}
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
    this.sections = JSON.parse(this.titles);
    Object.keys(this.sections).forEach(key => (this.entities[key] = []));
  },
  destroyed() {
    document.removeEventListener("click", this.documentClick);
  },
  methods: {
    getResult(searchText) {
      fetch(this.apiUrl + `?searchText=${searchText}`, {
        method: "GET",
        credentials: "include"
      })
        .then(response => {
          if (response.status !== 200) {
            throw new Error(response.status);
          }

          return response.json();
        })
        .then(data => {
          if (data.length > 0) {
            this.showDropdown = true;
          } else {
            this.showDropdown = false;
          }

          data.forEach(entity => {
            if (Object.keys(this.entities).includes(entity.ContentType)) {
              this.entities[entity.ContentType].push(entity);
            }
          });
        })
        .catch(() => {});
    },
    documentClick(e) {
      let el = this.$refs.dropdownResult;
      let target = e.target;
      if (el !== target && !el.contains(target)) {
        this.showDropdown = false;
      }
    }
  }
};
</script>
