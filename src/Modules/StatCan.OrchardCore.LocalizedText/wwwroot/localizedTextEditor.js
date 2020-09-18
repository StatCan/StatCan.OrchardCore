/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function initializeLocalizedTextEditor(element, confirmModalOptions) {
  var $el = $(element);
  var initialData = $el.data('init');
  var cultures = $el.data('cultures');
  $('.autoexpand-listener').on('input', 'textarea', function () {
    // textareas autoexpand when text changes
    this.style.height = 'auto';
    this.style.height = this.scrollHeight + 'px';
  });
  var parsedData = initialData ? initialData.map(function (i) {
    return {
      name: i.Name,
      localizedItems: i.LocalizedItems.map(function (j) {
        return {
          culture: j.Culture,
          value: j.Value,
          isCurrent: cultures.find(function (c) {
            return c == j.Culture;
          })
        };
      })
    };
  }) : []; // add missing cultures

  parsedData.forEach(function (item) {
    cultures.forEach(function (c) {
      if (!item.localizedItems.find(function (i) {
        return i.culture == c;
      })) {
        item.localizedItems.push({
          culture: c,
          isCurrent: true,
          value: ''
        });
      }
    });
  });
  return new Vue({
    el: element,
    data: {
      entries: []
    },
    computed: {
      value: function value() {
        this.entries;
        return JSON.stringify(this.entries);
      },
      hasEntries: function hasEntries() {
        return this.entries.length > 0;
      }
    },
    created: function created() {
      this.entries = parsedData;
    },
    mounted: function mounted() {
      // expand the textareas on mount
      $('.autoexpand-listener').find("textarea").each(function () {
        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';
      });
    },
    methods: {
      add: function add() {
        this.entries.push({
          name: '',
          localizedItems: cultures === null || cultures === void 0 ? void 0 : cultures.map(function (c) {
            return {
              culture: c,
              isCurrent: true,
              value: ''
            };
          })
        });
      },
      remove: function remove(index) {
        // use the admin confirm dialog before deleting
        var that = this;
        confirmDialog(_objectSpread(_objectSpread({}, confirmModalOptions), {}, {
          callback: function callback(resp) {
            if (resp) {
              that.entries.splice(index, 1);
            }
          }
        }));
      },
      updatePreview: function updatePreview() {
        this.$nextTick(function () {
          $(document).trigger('contentpreview:render');
        });
      }
    },
    watch: {
      entries: function entries() {
        this.updatePreview();
      }
    }
  });
}

;