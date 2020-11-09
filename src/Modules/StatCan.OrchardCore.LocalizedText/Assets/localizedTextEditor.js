function initializeLocalizedTextEditor(element, confirmModalOptions) {
  
  const $el = $(element);
  const initialData = $el.data('init');
  const cultures = $el.data('cultures');
  
  function autoExpand(){
    // textareas autoexpand when text changes
    this.style.height = 'auto';
    this.style.minHeight="38px";
    this.style.height = (this.scrollHeight) + 'px';
  }

  $('.autoexpand-listener').on('input', 'textarea', autoExpand);

  $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
    $('.autoexpand-listener').find("textarea").each(autoExpand);
  })

    let parsedData = initialData ?
        initialData.map(
            i => ({
                name: i.Name,
                localizedItems: i.LocalizedItems.map(j => ({
                    culture: j.Culture,
                    value: j.Value,
                    isCurrent: cultures.find(c => c == j.Culture)
                 }))
            })
        ) :
      [];
    // add missing cultures
    parsedData.forEach(item => {
      cultures.forEach(c => {
        if (!item.localizedItems.find(i => i.culture == c)) {
          item.localizedItems.push({ culture: c, isCurrent: true, value: '' });
          }
        });
    })


    return new Vue({
      el: element,

      data: {
          entries: [] ,
      },

      computed: {
        value: function () {
          this.entries
              return JSON.stringify(this.entries);
        },
        hasEntries() {
          return this.entries.length > 0;
        },
      },
      created: function () {
        this.entries = parsedData;
      },

      mounted: function () {
        // expand the textareas on mount
        $('.autoexpand-listener').find("textarea").each(autoExpand);
      },

      methods: {
        add: function () {
          this.entries.push({ name: '', localizedItems: cultures?.map(c => ({ culture: c, isCurrent: true, value: ''})) });
          },
           
        remove: function (index) {
          // use the admin confirm dialog before deleting
          var that = this;
          confirmDialog({
            ...confirmModalOptions, callback: function (resp) {
              if (resp) {
                that.entries.splice(index, 1);
              }
            }
          });  
          },
          updatePreview: function () {
              this.$nextTick(() => {
                  $(document).trigger('contentpreview:render');
              });
          }
      },

      watch: {
          entries: function () {
              this.updatePreview();
          }
      }
    });
};