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
| Background Image | Adds an image to the app-bar background |
| Bottom | Aligns the component towards the bottom. |
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

### VContainer

| Field  | Definition |
|--------|------------|
| Fluid | Removes viewport maximum-width size breakpoints. |
| Flow | Any number of `VRow`s can be added to the component |

### VDivider

| Field  | Definition |
|--------|------------|
| Dark | Applies the dark theme variant to the component. |
| Inset | Adds indentation (72px) for normal dividers, reduces max height for vertical. |
| Light | Applies the light theme variant to the component. |
| Vertical | Displays dividers vertically |

### VExpansionPanel

| Field  | Definition |
|--------|------------|
| Header | Sets the expansion-panel header. |
| Flow | Any number of widgets can be added to the component. |

### VExpansionPanels

| Field  | Definition |
|--------|------------|
| Accordion | Removes the margin around open panels. |
| Dark | Applies the dark theme variant to the component. |
| Disabled | Disables the entire expansion-panel. |
| Flat | Removes the expansion-panel’s elevation and borders. |
| Focusable | Makes the expansion-panel headers focusable. |
| Inset | Makes the expansion-panel open with a inset style. |
| Light | Applies the light theme variant to the component. |
| Mandatory | Forces a value to always be selected (if available). |
| Multiple | Allow multiple selections. |
| Popout | Makes the expansion-panel open with an popout style. |
| Read-Only | Makes the entire expansion-panel read only. |
| Tile | Removes the border-radius. |
| Flow | Any number of `VExpansionPanel`s can be added to the component |

### VFooter

This widget is best used in the **Footer** zone.

| Field  | Definition |
|--------|------------|
| Absolute | Applies **position: absolute** to the component. |
| App | Designates the component as part of the application layout. Used for dynamically adjusting content sizing. Components using this prop should reside **outside** of `v-main` component to function properly. You can find more information about layouts on the application page. **Note:** this prop automatically applies position: fixed to the layout element. |
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). You can find list of built in classes on the [colors page](https://dev.vuetifyjs.com/en/styles/colors/#material-colors). |
| Dark | Applies the dark theme variant to the component. | 
| Elevation | Designates an elevation applied to the component between 0 and 24. You can find more information on the [elevation page](https://dev.vuetifyjs.com/en/styles/elevation/). |
| Fixed | Applies **position: fixed** to the component. |
| Height | Designates a specific height for the component. |
| Light | Applies the light theme variant to the component. |
| Outlined | Makes the background transparent and applies a thin border. |
| Rounded | Designates the border-radius applied to the component. You can find more information on the [Border Radius page](https://vuetifyjs.com/en/styles/border-radius/). |
| Padless | Remove all padding from the footer. |
| Shaped | Applies a large border radius on the top left and bottom right of the footer. |
| Tile | Removes the component’s border-radius. |
| Width | Sets the width for the component. |

### VImg

| Field  | Definition |
|--------|------------|
| Alternate Text | Alternate text for screen readers. Leave empty for decorative images. |
| Aspect Ratio | Calculated as `width/height`, so for a 1920x1080px image this will be `1.7778`. Will be calculated automatically if omitted. |
| Contain | Prevents the image from being cropped if it doesn’t fit. |
| Dark | Applies the dark theme variant to the component. |
| Eager | Will force the components content to render on mounted. This is useful if you have content that will not be rendered in the DOM that you want crawled for SEO. |
| Gradient | Overlays a gradient onto the image. Only supports [linear-gradient](https://developer.mozilla.org/en-US/docs/Web/CSS/linear-gradient()) syntax, anything else should be done with classes (see examples). |
| Height | Sets the height for the component. |
| Light | Applies the light theme variant to the component. |
| Source | The image. This field is mandatory. |
| Width | Sets the width for the component. |

### VList

| Field  | Definition |
|--------|------------|
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). |
| Dark | Applies the dark theme variant to the component. |
| Dense | Lowers max height of list tiles.|
| Disabled | Disables all children **v-list-item** components |
| Elevation | Designates an elevation applied to the component between 0 and 24.  |
| Expand | Will only collapse when explicitly closed |
| Flat | Remove the highlighted background on active **v-list-items** |
| Height | Sets the height for the component. |
| Light | Applies the light theme variant to the component. |
| Nav | An alternative styling that reduces v-list-item width and rounds the corners. |
| Outlines | Removes elevation (box-shadow) and adds a thin border. |
| Rounded | Rounds the v-list-item edges. |
| Shaped | Provides an alternative active style for v-list-item. |
| Subheader | Removes top padding. Used when previous sibling is a header. |
| Tag | Specify a custom tag used on the root element. |
| Tile | Removes the component’s border-radius. |
| Width | Sets the width for the component. |

### VListItem

| Field  | Definition |
|--------|------------|
| Active-Class | Configure the active CSS class applied when the link is active. |
| Color | Applies specified color to the control when in an active state or input-value is true - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)) |
| Exact-Active-Class | Configure the active CSS class applied when the link is active with exact match. |
| Href | Designates the component as anchor and applies the href attribute. |
| Icon Name | Designates a specific icon. |
| Input-Value | Controls the active state of the item. This is typically used to highlight the component. |
| Target | Designates the target attribute. This should only be applied when using the href prop. |
| To | Denotes the target route of the link. |

### VNavigationDrawer

This widget is best used in the **Navigation Drawer** zone.

| Field  | Definition |
|--------|------------|
| Absolute | Applies **position: absolute** to the component. |
| Background Image | Adds an image to the navigation-drawer background |
| Bottom | Expands from the bottom of the screen on mobile devices. |
| Clipped | A clipped drawer rests under the application toolbar. |
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). You can find list of built in classes on the [colors page](https://dev.vuetifyjs.com/en/styles/colors/#material-colors). |
| Disable Resize Watcher | Will automatically open/close drawer when resized depending if mobile or desktop. | 
| Disable Route Watcher | Disables opening of navigation drawer when route changes. |
| Expand On Hover | Collapses the drawer to a **mini-variant** until hovering with the mouse. |
| Floating | A floating drawer has no visible container (no border-right). |
| Height | Sets the height of the navigation drawer. |
| Hide Overlay | Hides the display of the overlay. |
| Light | Applies the light theme variant to the component. |
| Mini | Condenses navigation drawer width, also accepts the **.sync** modifier. With this, the drawer will re-open when clicking it. |
| Mini Variant Width | Designates the width assigned when the **mini** prop is turned on. |
| Mobile Breakpoint | Sets the designated mobile breakpoint for the component. This will apply alternate styles for mobile devices such as the **temporary** prop, or activate the **bottom** prop when the breakpoint value is met. Setting the value to 0 will disable this functionality. |
| Overlay Color | Sets the overlay color. |
| Overlay Opacity | Sets the overlay opacity. |
| Permanent | The drawer remains visible regardless of screen size. |
| Right | Places the navigation drawer on the right. |
| Stateless | Remove all automated state functionality (resize, mobile, route) and manually control the drawer state. |
| Temporary | A temporary drawer sits above its application and uses a scrim (overlay) to darken the background. |
| Touchless | Disable mobile touch functionality. |
| Width | Sets the width for the component. |

### VRow

| Field  | Definition |
|--------|------------|
| Align [Breakpoint] | Applies the [align-items](https://developer.mozilla.org/en-US/docs/Web/CSS/align-items) css property. Available options are **start**, **center**, **end**, **baseline** and **stretch**. |
| Align Content [Breakpoint] | Applies the [align-content](https://developer.mozilla.org/en-US/docs/Web/CSS/align-content) css property. Available options are **start**, **center**, **end**, **space-between**, **space-around** and **stretch**. |
| Dense | Reduces the gutter between `VCol`s. |
| Justify [Breakpoint] | Applies the [justify-content](https://developer.mozilla.org/en-US/docs/Web/CSS/justify-content) css property. Available options are **start**, **center**, **end**, **space-between** and **space-around**. |
| No Gutter | Removes the gutter between `VCol`s. |
| Flow | Any number of `VCol`s can be added to the component. |

### VSubheader

| Field  | Definition |
|--------|------------|
| Dark | Applies the dark theme variant to the component. |
| Inset | Adds indentation (72px) |
| Light | Applies the light theme variant to the component. |

### VTimeLine

| Field  | Definition |
|--------|------------|
| Align-Top | Align caret and dot of timeline items to the top. |
| Dark | Applies the dark theme variant to the component. |
| Dense | Hide opposite slot content, and position all items to one side of timeline. |
| Light | Applies the light theme variant to the component. |
| Reverse | Reverse direction of timeline items. |

### VTimeLineItem

| Field  | Definition |
|--------|------------|
| Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). |
| Dark | Applies the dark theme variant to the component. |
| Icon | Specify icon for dot container |
| Icon-Color | Applies specified color to the control - it can be the name of material color (for example success or purple) or css color (#033 or rgba(255, 0, 0, 0.5)). |
| Fill-Dot | Remove padding from dot container |
| Hide-Dot | Hide display of timeline dot |
| Large | Large size dot |
| Left | Explicitly set the item to a left orientation |
| Light | Applies the light theme variant to the component. |
| Right | Explicitly set the item to a right orientation. |
| Small | Small size dot. |