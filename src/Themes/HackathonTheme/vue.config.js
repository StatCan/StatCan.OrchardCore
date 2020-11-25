module.exports = {
  "transpileDependencies": [
    "vuetify"
  ],
  outputDir: "wwwroot/dist",
  configureWebpack(config) {
    config.plugins.some((plugin, index) => {
        return plugin.options?.filename === 'demo.html' ? config.plugins.splice(index, 1) : false;
    });
    
    return {}; // your config
  },
}
