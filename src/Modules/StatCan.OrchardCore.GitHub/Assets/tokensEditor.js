function initializeTokensEditor(elem, data) {

  console.log(data)
    var store = {
        debug: false,
        state: {
            tokens: data
        },
        addToken: function () {
            if (this.debug) { console.log('add token triggered') };
            this.state.tokens.push({ name: '', value: '' });
        },
        removeToken: function (index) {
            if (this.debug) { console.log('remove token triggered with', index) };
            this.state.tokens.splice(index, 1);
        },
    }

    var tokensTable = {
        template: '#tokens-table',
        props: ['data'],
        name: 'tokens-table',
        methods: {
            add: function () {
              store.addToken();
            },
            remove: function (index) {
              store.removeToken(index);
          },
          getTokensFormattedList: function () {
            if (this.debug) { console.log('getTokensFormattedList triggered') };
            return JSON.stringify(store.state.tokens.filter(function (x) { return !IsNullOrWhiteSpace(x.name) }));
          }
        }
    };

    new Vue({
        components: {
            tokensTable: tokensTable,
        },
        data: {
            sharedState: store.state
        },
        el: elem,
    });

}
function IsNullOrWhiteSpace(str) {
  return str === null || str.match(/^ *$/) !== null;
}
