# VueForms (`StatCan.OrchardCore.VueForms`)

The VueForms module aims to simplify the creation of client side forms in OrchardCore.

## Content definitions

### VueForm

The VueForm content type is used to create forms that use [VueJs](https://vuejs.org/), [Vuetify](https://vuetifyjs.com/en/getting-started/quick-start/) and [VeeValidate](https://logaretm.github.io/vee-validate/) client side librairies.
The form submission is handled via an ajax call to a generic controller that returns json to the client for seamless server side validation.

### VueForm Part

| Field  | Definition |
|--------|------------|
| Enabled | If disabled, the DisplayHtml field value will be displayed and the VueFormController will return a 404 for form submissions. |
| DisabledHtml | Html displayed when the form is disabled. |
| SuccessMessage | The success message returned to the client when the form is valid and no redirect is specified after submission. With Liquid support. |
| ErrorMessage | The error message returned to the client when the form is invalid. With Liquid support. |

### VueFormScripts Part

| Field  | Definition |
|--------|------------|
| ClientInit | A client side script that is executed prior to instanciating the VueJs app. |
| OnValidation | Server side validation script that allows us to validate the form. |
| OnSubmitted  | Server side script that runs if the form is valid |


#### ClientInit script

The ClientInit script is executed client side prior to instanciating the VueJS app. This script has access to the client's global scope variables.

This was added to support hooking into some global options for VeeValidate.

For example, setting the [VeeValidate.setInteractionMode('passive')](https://logaretm.github.io/vee-validate/guide/interaction-and-ux.html) option 
or the [Localization](https://logaretm.github.io/vee-validate/guide/localization.html) option.

It is also possible to modify the vuetify [global-config](https://vuetifyjs.com/en/customization/global-config/) or [presets](https://vuetifyjs.com/en/customization/presets/). TODO: Document how


#### OnValidation script

The OnValidation script is used to specify the server side valiadtion script. We are planning to implement components with integrated validation in the future.
This module adds some [scripting methods](../Scripting.md) to facilitate handling form data and errors. 

Here is an example OnValidation script that validates that the name is required.

``` javascript 
var data = requestFormAsJsonObject();

if(data.name == "") {
 addError('name', 'The name is required'); 
}
```

Of course, you can also use this script to perform advanced validations such as validating duplicate names, or calling an external service.

#### OnSubmitted script

The OnSubmitted script is executed after the OnValidation script, only if the form is valid. 
This is where you would typically create a contentItem from the form data or redirect the user to another page.

Here is an example OnSubmitted script that redirects the user to a success page

``` javascript 
httpContext().Response.Redirect('/some-url')
```

Here is another example on how to create a ContentItem from the form data
``` javascript 
TODO
```

### VueComponent Widget

The VueComponent widget is meant to be used inside a VueForm and allows a user to write a vuejs component with a custom name, template and script.

...

## Scripting

Please see the [scripting](../Scripting.md) documentation.

## Liquid

TODO: Add liquid filter documentation

Please see the [liquid](../Liquid.md.md) documentation.

## Workflow integration
TODO: Add workflow documentation

Although this module does not require a workflow to work and handle the form submissions, we still provide a workflow hook to support performing some additional actions.

Please see the [workflow](../Workflow.md) documentation.

## Example form lifecycle

TODO


## FAQ

TODO
### How to perform a Http redirect when the form is valid

### How to create a content item from form data

### How to load previous data (edit some existing content item data from the frontend)

## Notes / gotchas

- Your form inputs need to have a name for them to be sent to the server.
- Form server side validation needs to be 

