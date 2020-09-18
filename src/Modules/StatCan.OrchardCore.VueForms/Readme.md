# VueForms (`StatCan.OrchardCore.VueForms`)

The VueForms module aims to simplify the creation of client side forms in OrchardCore.

## Content definitions

### VueForm

The VueForm content type is used to create forms that use [VueJs](https://vuejs.org/), [Vuetify](https://vuetifyjs.com/en/getting-started/quick-start/) and [VeeValidate](https://logaretm.github.io/vee-validate/) client side librarires.
The form submission is handled via an ajax call to a generic controller that returns json to the client for seamless server side validation.

### VueForm Part

| Field  | Definition |
|--------|------------|
| Enabled | If disabled, the VueFormController will return a 404 for form submissions. |
| DisabledHtml   | Html displayed when the form is disabled. |
| SuccessMessage   | The success message returned to the client when the form is valid and no redirect is specified after submission. |
| ErrorMessage   | The error message returned to the client when the form is invalid. |

### VueFormScripts Part

| Field  | Definition |
|--------|------------|
| ClientInit | A client side script that is executed prior to the VueJs app  |
| OnValidation | Server side validation script that allows us to validate the form. |
| OnSubmitted   | Server side script that runs if the form is valid |


#### ClientInit script

The ClientInit script is executed client side prior to instanciating the VueJS app. This script has access to Global JS variables.

This was added to support hooking into some global options for VeeValidate.

For example, setting the [VeeValidate.setInteractionMode('passive')](https://logaretm.github.io/vee-validate/guide/interaction-and-ux.html) option 
or the [Localization](https://logaretm.github.io/vee-validate/guide/localization.html) options.

It is also possible to modify the vuetify [global-config](https://vuetifyjs.com/en/customization/global-config/) or [presets](https://vuetifyjs.com/en/customization/presets/).


#### OnValidation script

The OnValidation script is meant to be used to specify the server side valiadtion script.
In this script, you have access to all Orchard's [scripting methods](https://docs.orchardcore.net/en/dev/docs/reference/modules/Scripting/#methods). 
This module adds some methods (see below) to facilitate handling form data and errors. When trying to use a Scripting method and getting an error, make sure that the feature it's contained in is enabled

Here is an example OnValidation script that validates that the name is required.

``` javascript 
var data = requestFormAsJsonObject();

if(data.name == "") {
 addError('name', 'The name is required'); 
}
```

Of course, you can also use this script to perform advanced validations such as validating duplicates, or calling an external service.


## Scripting

OC [scripting methods](https://docs.orchardcore.net/en/dev/docs/reference/modules/Scripting/#methods) docs can be viewed here:

| Function | Description 
| -------- | ----------- |
| `addError(): void` | Returns true if the current request Url is the current homepage |
| `getFormContentItem(): ContentItem` | Returns true if there is no authenticated user on the current request |

## Liquid

## Workflow integration

## Examples

## FAQ

### How to hook into a workflow

### How to perform a Http redirect when the form is valid

### How to create a content item from form data

### How to load previous data (edit some existing data from the frontend)

## Caviats

- Your form inputs need to have a name
- 

