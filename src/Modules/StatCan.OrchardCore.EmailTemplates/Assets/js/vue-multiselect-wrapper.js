function debounce(func, wait, immediate) {
    var timeout;
    return function () {
        var context = this, args = arguments;
        var later = function () {
            timeout = null;
            if (!immediate) func.apply(context, args);
        };
        var callNow = immediate && !timeout;
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
        if (callNow) func.apply(context, args);
    };
};
function initVueMultiselect(element) {
    // only run script if element exists
    if (element) {
        var elementId = element.id;
        var selectedItems = JSON.parse(element.dataset.selectedItems || "[]");
        if (selectedItems == null){
          selectedItems = [];
        }
        var multiple = JSON.parse(element.dataset.multiple);
        var allItems = JSON.parse(element.dataset.allItems || "[]");

        var debouncedSearch = debounce(function (vm, query) {
            vm.isLoading = true;
        }, 250);

        var vueMultiselect = Vue.component('vue-multiselect', window.VueMultiselect.default);

        var vm = new Vue({
            el: '#' + elementId,
            components: { 'vue-multiselect': vueMultiselect },
            data: {
                value: null,
                arrayOfItems: selectedItems,
                options: allItems,
            },
            computed: {
                selectedIds: function () {
                    return this.arrayOfItems.map(function (x) { return x.Value }).join(',');
                },
                isDisabled: function () {
                    return this.arrayOfItems.length > 0 && !multiple;
                }
            },
            watch: {
                selectedIds: function () {
                    // We add a delay to allow for the <input> to get the actual value	
                    // before the form is submitted	
                    setTimeout(function () { $(document).trigger('contentpreview:render') }, 100);
                }
            },
            methods: {
                onSelect: function (selectedOption) {
                    var self = this;

                    for (i = 0; i < self.arrayOfItems.length; i++) {
                        if (self.arrayOfItems[i].Value === selectedOption.Value) {
                            return;
                        }
                    }

                    self.arrayOfItems.push(selectedOption);
                },
                remove: function (item) {
                    this.arrayOfItems.splice(this.arrayOfItems.indexOf(item), 1)
                }
            }
        })
        
        /*Hook for other scripts that might want to have access to the view model*/
        var event = new CustomEvent("vue-multiselect-created", { detail: { vm: vm } });
        document.querySelector("body").dispatchEvent(event);
    }
}
