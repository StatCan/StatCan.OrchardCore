# Vuetify Theme

The Vuetify Theme aims to simplify the creation of client side apps in OrchardCore. Offering vuetify components as widgets, the Vuetify Theme makes it easy to compose rich Material Design pages from the Admin UI.

## Widgets

### VAlert

| Field  | Definition |
|--------|------------|
| Border | Puts a border on the alert. Accepts **top**, **right**, **bottom**, **left**. |
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). You can find list of built in classes on the [colors page](https://dev.vuetifyjs.com/en/styles/colors/#material-colors). |
| Colored Border | Applies the defined color to the alert’s border. |
| Dark | Applies the dark theme variant to the component. | 
| Dense | Hides the alert icon and decreases component’s height. |
| Dismissible | Adds a close icon that can hide the alert. |
| Elevation | Designates an elevation applied to the component between 0 and 24. You can find more information on the [elevation page](https://dev.vuetifyjs.com/en/styles/elevation/). |
| Icon | Designates a specific icon. |
| Light | Applies the light theme variant to the component. |
| Outlined | Makes the background transparent and applies a thin border. |
| Prominent | Displays a larger vertically centered icon to draw more attention. |
| Shaped | Applies a large border radius on the top left and bottom right of the card. |
| Text | Applies the defined color to text and a low opacity background of the same. |
| Tile | Removes the component’s border-radius. |
| Type | Specify a **success**, **info**, **warning** or **error** alert. Uses the contextual color and has a pre-defined icon. |

### VAppBar

This widget is best used in the **Header** zone.

| Field  | Definition |
|--------|------------|
| Absolute | Applies **position: absolute** to the component. |
| Bottom | Aligns the component towards the bottom. |
| Background Image | Adds an image to the app-bar background |
| Collapse | Puts the toolbar into a collapsed state reducing its maximum width. |
| Collapse On Scroll | Puts the app-bar into a collapsed state when scrolling. |
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). You can find list of built in classes on the [colors page](https://dev.vuetifyjs.com/en/styles/colors/#material-colors). |
| Dark | Applies the dark theme variant to the component. | 
| Dense | Reduces the height of the toolbar content to 48px (96px when using the **prominent** prop). |
| Elevate On Scroll | Elevates the app-bar when scrolling. |
| Elevation | Designates an elevation applied to the component between 0 and 24. You can find more information on the [elevation page](https://dev.vuetifyjs.com/en/styles/elevation/). |
| Extended | Use this prop to increase the height of the toolbar *without* using the `extension` slot for adding content. May be used in conjunction with the **extension-height** prop, and any of the other props that affect the height of the toolbar, e.g. **prominent**, **dense**, etc., WITH THE EXCEPTION of **height**. |
| Extension Height | Specify an explicit height for the `extension` slot. |
| Fade Image On Scroll | When using the *src* prop or `img` slot, will fade the image when scrolling. |
| Fixed | Applies **position: fixed** to the component. |
| Flat | Removes the toolbar’s box-shadow. |
| Floating | Applies **display: inline-flex** to the component. |
| Height | Designates a specific height for the toolbar. Overrides the heights imposed by other props, e.g. **prominent**, **dense**, **extended**, etc. |
| Hide On Scroll | Hides the app-bar when scrolling. Will still show the `extension` slot. |
| Inverted Scroll | Hides the app-bar when scrolling down and displays it when scrolling up. |
| Light | Applies the light theme variant to the component. |
| Outlined | Makes the background transparent and applies a thin border. |
| Prominent | Increases the height of the toolbar content to 128px. |
| Scroll Off Screen | Hides the app-bar when scrolling. Will **NOT** show the `extension` slot. |
| Scroll Threshold | The amount of scroll distance down before **hide-on-scroll** activates. |
| Shaped | Applies a large border radius on the top left and bottom right of the app-bar. |
| Short | Reduce the height of the toolbar content to 56px (112px when using the prominent prop). |
| Shrink On Scroll | Shrinks a **prominent** toolbar to a **dense** or **short** (default) one when scrolling. |
| Tile | Removes the component’s border-radius. |
| Width | Sets the width for the component. |

### VCard

| Field  | Definition |
|--------|------------|
| Title | Applies a header to the card. |
| Subtitle | Applies a subheader to the card. |
| Content | Allows free-form HTML. |
| Actions | Call-to-action buttons for the card. |

### VCol

| Field  | Definition |
|--------|------------|
| Align Self |  Applies the [align-items](https://developer.mozilla.org/en-US/docs/Web/CSS/align-items) css property. Available options are **start**, **center**, **end**, **auto**, **baseline** and **stretch**. |
| Cols [Breakpoint] | Sets the default number of columns the component extends. Available options are **1 -> 12** and **auto**. |
| Offset [Breakpoint] | Sets the default offset for the column. |
| Order [Breakpoint] | Sets the default [order](https://developer.mozilla.org/en-US/docs/Web/CSS/order) for the column. |
| Flow | Any number of widgets can be added to the component. |

### VRow

| Field  | Definition |
|--------|------------|
| Align [Breakpoint] | Applies the [align-items](https://developer.mozilla.org/en-US/docs/Web/CSS/align-items) css property. Available options are **start**, **center**, **end**, **baseline** and **stretch**. |
| Align Content [Breakpoint] | Applies the [align-content](https://developer.mozilla.org/en-US/docs/Web/CSS/align-content) css property. Available options are **start**, **center**, **end**, **space-between**, **space-around** and **stretch**. |
| Dense | Reduces the gutter between `VCol`s. |
| Justify [Breakpoint] | Applies the [justify-content](https://developer.mozilla.org/en-US/docs/Web/CSS/justify-content) css property. Available options are **start**, **center**, **end**, **space-between** and **space-around**. |
| No Gutter | Removes the gutter between `VCol`s. |
| Flow | Any number of `VCol`s can be added to the component. |