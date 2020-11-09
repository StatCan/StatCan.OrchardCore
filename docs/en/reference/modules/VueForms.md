# VueForms (`StatCan.OrchardCore.VueForms`)

The VueForms module aims to simplify the creation of client side forms in OrchardCore.

## Content definitions

### VueForm

The VueForm content type is used to create forms that use [VueJs](https://vuejs.org/) and [VeeValidate](https://logaretm.github.io/vee-validate/) client side librairies.
The form submission is handled via an ajax call to a generic controller that returns json to the client for seamless server side validation.

### VueForm Part

| Field  | Definition |
|--------|------------|
| Enabled | If disabled, the DisplayHtml field value will be displayed and the VueFormController will return a 404 for form submissions. |
| RenderAs | Render the Form as a Vue Component, a Vue App or Vuetify app |
| DisabledHtml | Html displayed when the form is disabled. |
| SuccessMessage | The success message returned to the client when the form is valid and no redirect is specified after submission. With Liquid support. |

### VueFormScripts Part

| Field  | Definition |
|--------|------------|
| ClientInit | A client side script that is executed prior to instanciating the VueJs app. |
| ComponentOptions | The VueJS component options object for your VueForm component. |
| OnValidation | Server side validation script that allows us to validate the form. |
| OnSubmitted  | Server side script that runs if the form is valid |


#### ClientInit script

The ClientInit script is executed client side prior to instanciating the form component. This script has access to the client's global scope variables.

This was added to support hooking into some global options for VeeValidate.

For example, setting the [VeeValidate.setInteractionMode('passive')](https://logaretm.github.io/vee-validate/guide/interaction-and-ux.html) option 
or the [Localization](https://logaretm.github.io/vee-validate/guide/localization.html) option.


```javascript
VeeValidate.setInteractionMode('passive');
```

#### OnValidation script

The OnValidation script is used to specify the server side valiadtion script. We are planning to implement components with integrated validation in the future.
This module adds some [scripting methods](../Scripting.md#vueforms-module-statcanorchardcorevueforms) to facilitate handling form data and errors. 

Here is an example OnValidation script that validates that the name is required.

**Note**: the first parameter of addError must be your input name that must also match the VeeValidate ValidationProvider name.

``` javascript 
var data = requestFormAsJsonObject();

if(data.name == "") {
 addError('name', 'The name is required'); 
}
```

Of course, you can also use this script to perform advanced validations such as validating duplicate names, or calling an external service.

The server side validation depends on your vue component using VeeValidate's ValidationProvider component to map error messages.

#### OnSubmitted script

The OnSubmitted script is executed after the OnValidation script, only if the form is valid. 
This is where you would typically create a contentItem from the form data or redirect the user to another page.

Here is an example OnSubmitted script that redirects the user to a success page after having created the PersonInfo content item. **Note**: the `OrchardCore.Workflow.Http` module must be enabled to have access to the `httpContext()` method.

``` javascript 
var data = requestFormAsJsonObject();

createContentItem("PersonInfo", true, {
  "PersonInfo": {
    "Name": {
      "Text": data.name
    },
    "Email": {
      "Text": data.email
    },
    "Gender": {
      "Text": data.gender
    }
  }
});

httpContext().Response.Redirect('/success')
```

#### Component Options object

This is where you write the[VueJS component options object](https://012.vuejs.org/api/options.html) for the form component. At a minimum, you must define the data object. When the VueForm is submitted, the data object is serialized and sent to the server.

```javascript
{
  data: () => ({
    items: ["Female", "Male", "Other"],
    name: "",
    email: "",
    gender: undefined
  })
}
```


### VueComponent Widget

The VueComponent widget allows you to write VueJS markup that will be included in your VueForm component

#### Template

**Important implementation notes:**
- This field should return a **single** vue / html node.

Example:

```html
<v-container >
  <v-card-text>
    <v-row justify="center">
      <v-col cols="8">
        <validation-provider name="{{ "nameLabel" | localize | downcase }}" rules="required" v-slot="{ errors }">
          <v-text-field v-model="name" :error-messages="errors" filled="filled" label="{{ "nameLabel" | localize }}"></v-text-field>
        </validation-provider>
      </v-col>
      <v-col cols="8">
        <validation-provider name="{{ "emailLabel" | localize | downcase }}" rules="required|email" v-slot="{ errors }">
          <v-text-field v-model="email" :error-messages="errors" filled="filled" label="{{ "emailLabel" | localize }}"></v-text-field>
        </validation-provider>
      </v-col>
      <v-col cols="8">
        <validation-provider name="{{ "messageLabel" | localize | downcase }}" rules="required" v-slot="{ errors }">
       	  <v-textarea v-model="message" :error-messages="errors" counter="true" filled="filled" label="{{ "messageLabel" | localize }}" rows="5">
          </v-textarea>
    	</validation-provider>
      </v-col>
      <v-col cols="8">
        <v-alert type="success" v-if="form.successMessage">
          {% raw %}{{ form.successMessage }}{% endraw %}
        </v-alert>
        <v-alert type="error" v-if="form.errorMessage">
          {% raw %}{{ form.errorMessage }}{% endraw %}
        </v-alert>
      </v-col>
      <v-col cols="8">
    	<v-btn type="submit" depressed block @click="formHandleSubmit" :disabled="form.submitting">{{ "submitLabel" | localize }}</v-btn>
      </v-col>
    </v-row>
  </v-card-text>
</v-container>
```


## Available Props

The VueForm component has some default properties and methods.

You can access these properties in your templates or in the component options object.

|  Name  | Definition |
|--------|------------|
| obs.* | All props available on the v-slot of the [ValidationObserver](https://logaretm.github.io/vee-validate/api/validation-observer.html#scoped-slot-props) are available. |
| formReset | A method that resets the form.* properties to the initial state. Does not reset your component's data. `() => void` |
| formHandleSubmit | A method that calls the `validate()` method and then, if valid, sends an ajax request to our controller `() => void` |
| form.submitting | Set to true when the form is being submitted. |
| form.submitSuccess | Set to true when no redirect is specified and the submission was a success. |
| form.successMessage | The success message returned from the server as specified in the [VueForm](#vueform-part) |
| form.submitValidationError | Set to true when a server validation error occus. |
| form.serverValidationMessage | Array of errors set by the server with the `addError('serverValidationMessage', 'Message')` scripting method. |
| form.submitError | Set to true when a server error, ajax error or unhandled error occurs. |
| form.serverErrorMessage | An error message set with the ajax error status code and text. Only set when a server errors occur. |


## Scripting

Please see the [scripting](../Scripting.md#vueforms-module-statcanorchardcorevueforms) documentation.

## Workflow integration

Although this module does not require a workflow to handle the form submissions, we still provide a workflow hook to support performing some additional actions.

Please see the [workflow](../Workflows.md#vueforms-statcanorchardcorevueforms) documentation.

## Localization (`StatCan.OrchardCore.VueForms.Localized`)

While you can use Orchard's [LocalizationPart](https://docs.orchardcore.net/en/dev/docs/reference/modules/ContentLocalization/#localizationpart) to localize your forms. We suggest you use the [LocalizedText](LocalizedText.md) feature to implement i18n in your forms. This part is what we weld to your VueForm content type when you enable this feature.

## Examples

The following example forms are provided with the VueForms module as recipes:

- Contact Form
