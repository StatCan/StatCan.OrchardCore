# VueForms (`StatCan.OrchardCore.VueForms`)

The VueForms module aims to simplify the creation of client side forms in OrchardCore.

## Content definitions

### VueForm

The VueForm content type is used to create forms that use [VueJs](https://vuejs.org/) and [VeeValidate](https://logaretm.github.io/vee-validate/) client side libraries.
The form submission is handled via an ajax call to a generic controller that returns json to the client for seamless server side validation.

### VueForm Part

| Field          | Definition                                                                                                                            |
| -------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| Disabled       | If disabled, the `DisabledHtml` field value will be displayed instead of the form                                                     |
| RenderAs       | Render the Form as a Vue Component, a Vue App or Vuetify app                                                                          |
| DisabledHtml   | Html displayed when the form is disabled.                                                                                             |
| SuccessMessage | The success message returned to the client when the form is valid and no redirect is specified after submission. With Liquid support. |
| Template       | The component template. With Liquid support.                                                                                          |
| Debug mode     | Enables debug mode on the form.                                                                                                       |

_Note about RenderAs_: When rendering the Form as a `VueComponent`, you need to define a Zone in your layout called `DynamicComponentZone`. This zone must be rendered anywhere before the `FootScript` resource zone.
This is required because the div that defines the VueForm component needs to be defined outside of the Vue App. Please look at our open source [VuetifyTheme](https://github.com/StatCan/StatCan.OrchardCore/blob/master/src/Themes/VuetifyTheme/Views/Layout.liquid) layout for an example.

```
{% render_section "DynamicComponentZone", required: false %}
{% resources type: "FootScript" %}
```

#### Template

The template of the vue component

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

##### Available Props

The VueForm component has some default properties and methods.

You can access these properties in your templates or in the component options object.

| Name                         | Definition                                                                                                                                                           |
| ---------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| obs.\*                       | All props available on the v-slot of the [ValidationObserver](https://logaretm.github.io/vee-validate/api/validation-observer.html#scoped-slot-props) are available. |
| formReset                    | A method that resets the form.\* properties to the initial state. Does not reset your component's data. `() => void`                                                 |
| formHandleSubmit             | A method that calls the `validate()` method and then, if valid, sends an ajax request to our controller `() => void`                                                 |
| form.submitting              | Set to true when the form is being submitted.                                                                                                                        |
| form.submitSuccess           | Set to true when no redirect is specified and the submission was a success.                                                                                          |
| form.successMessage          | The success message returned from the server as specified in the [VueForm](#vueform-part)                                                                            |
| form.submitValidationError   | Set to true when a server validation error occus.                                                                                                                    |
| form.serverValidationMessage | Array of errors set by the server with the `addError('serverValidationMessage', 'Message')` scripting method.                                                        |
| form.responseData            | The raw response data recieved from the server. Useful if you want to return additional data to the form via a workflow.                                             |
| form.submitError             | Set to true when a server error, ajax error or unhandled error occurs.                                                                                               |
| form.serverErrorMessage      | An error message set with the ajax error status code and text. Only set when a server errors occur.                                                                  |

#### Debug mode

Debug mode is typically used when developping a form. Additional information is returned by the server and any data output by the `debug()` scripting method is also output and returned with the HttpResponse. This mode does not work when previewing the form.

### VueFormScripts Part

| Field            | Definition                                                                  |
| ---------------- | --------------------------------------------------------------------------- |
| ClientInit       | A client side script that is executed prior to instanciating the VueJs app. |
| ComponentOptions | The VueJS component options object for your VueForm component.              |
| OnValidation     | Server side validation script that allows us to validate the form.          |
| OnSubmitted      | Server side script that runs if the form is valid                           |

#### ClientInit script

The ClientInit script is executed client side prior to instanciating the form component. This script has access to the client's global scope variables.

This was added to support hooking into some global options for VeeValidate.

For example, setting the [VeeValidate.setInteractionMode('passive')](https://logaretm.github.io/vee-validate/guide/interaction-and-ux.html) option
or the [Localization](https://logaretm.github.io/vee-validate/guide/localization.html) option.

```javascript
VeeValidate.setInteractionMode("passive");
```

#### OnValidation script

The OnValidation script is used to specify the server side validation script. We are planning to implement components with integrated validation in the future.
This module adds some [scripting methods](../Scripting.md#vueforms-module-statcanorchardcorevueforms) to facilitate handling form data and errors.

Here is an example OnValidation script that validates that the name is required.

**Note**: the first parameter of addError must be your input name that must also match the VeeValidate ValidationProvider name.

```javascript
var data = requestFormAsJsonObject();

if (data.name == "") {
  addError("name", "The name is required");
}
```

All errors are also available to the client Vue component via the `form.responseData.errors` property

#### OnSubmitted script

The OnSubmitted script is executed after the OnValidation script, only if the form is valid.
This is where you would typically create a contentItem from the form data or redirect the user to another page.

Here is an example OnSubmitted script that redirects the user to a success page after having created the PersonInfo content item.

```javascript
var data = requestFormAsJsonObject();

createContentItem("PersonInfo", true, {
  PersonInfo: {
    Name: {
      Text: data.name
    },
    Email: {
      Text: data.email
    },
    Gender: {
      Text: data.gender
    }
  }
});
// redirects to the success page
httpRedirect("~/success");
```

#### Component Options object

This is where you write the[VueJS component options object](https://012.vuejs.org/api/options.html) for the form component. At a minimum, you must define the data object. By default, the entire data object is serialized and sent to the server when the form is submitted.

```javascript
{
  data: () => ({
    items: ["Female", "Male", "Other"],
    name: "",
    email: "",
    gender: undefined
  });
}
```

You can also overwrite the `submitData()` method to customize what is sent to the server. This can be very useful when your `data` object contains list of items for a dropdown.

```javascript
{
  data: () => ({
    items: ["Female", "Male", "Other"],
    name: "",
    email: "",
    gender: undefined
  }),
  submitData() {
    return { name: this.name, email: this.email, gender: this.gender };
  },
}
```

## Custom Form Components

The sections below decribes the custom form components provided by this module.

### Multiselect components

The module exposes two multiselect components: `multiselect-async` and `multiselect`. These components are wrappers of the multiselect component from the [vue-multiselect](https://vue-multiselect.js.org/) library. The api of the base `vue-multiselect` component can be found [here](https://vue-multiselect.js.org/#sub-getting-started).

#### multiselect-async component

The `multiselect-async` component allows async loading of options from an api.

| Props          | Definition                                          |
| -------------- | --------------------------------------------------- |
| value          | The holder of the selected options                  |
| api            | The url of the api that the options are coming from |
| success        | A boolean representing the validation result        |
| error-messages | Messages to show when validation fails              |

All the other props are the same as the base multiselect component.

#### multiselect component

The `multiselect` component provides a multiselect feature without async loading of options.

| Props          | Definition                                   |
| -------------- | -------------------------------------------- |
| value          | The holder of the selected options           |
| success        | A boolean representing the validation result |
| error-messages | Messages to show when validation fails       |

All the other props are the same as the base multiselect component.

### Bag component

The `bag` component provides a bag behaviour to the form. A bag is a collection of field groups. The bag component behaves similar to the BagPart in Orchard Core.

| Props               | Definition                                                             |
| ------------------- | ---------------------------------------------------------------------- |
| value               | The holder of the selected options over the entire bag                 |
| valueNames          | The name of the value object of each individual field in a field group |
| add-button-label    | The label of the add button                                            |
| remove-button-label | The label of the remove button                                         |
| success             | A boolean representing the validation result.                          |
| errorMessages       | Messages to show when validation fails                                 |

Below is an example of the `value` object:

```javascript
value: [{ field1: "field1Value", field2: "field2Value" }];
```

The general form of the `value` object is an array of objects. Each object represents a group of fields. Each proprety represents the value of a specific field.

The `valueNames` prop is a string array of the proprety name used in the field group objects in the `value` object. For example, the `valueNames` array corresponding to the `value` object given above is

```javascript
valueNames: ["field1", "field2"];
```

#### Bag component example

Here is an example of using the `bag` component

```html
<validation-provider name="Bag" rules="required" v-slot="{ errors, valid }">
  <bag
    v-model="bagValues"
    :valueNames="valueNames"
    :success="valid"
    :error-messages="errors"
    add-button-label="Add"
    remove-button-label="-"
  >
    <template #validations>
      <validation-provider
        rules="required"
        name="Field 1"
      ></validation-provider>
      <validation-provider
        rules="required"
        name="Field 2"
      ></validation-provider>
    </template>

    <template #components>
      <v-text-field filled="filled" label="label"> </v-text-field>
      <v-text-field filled="filled" label="label"> </v-text-field>
    </template>

    <template #title>
      <div class="text-h5 mb-5">
        Bag
      </div>
    </template>
  </bag>
</validation-provider>
```

The data object is:

```javascript
{
    data: () => ({
        bagValues: [{ field1: "field1", field2: "field2" }],
        valueNames: ["field1", "field2"]
    })
}
```

The `bag` component can be wrapped in a `validation-provider` just like any other field components. 

The `bag` component has three slots. The `validations` slots contains the `validation-provider` for each field in the field groups. The `components` slot contains the actual field components. The `title` slot contains components representing the title of the `bag` component.   

The components in the `validations` and `components` slots are prototypes. For example, whenever the add button is clicked, the components defined in those two slots will be "instantiated". 

##### Some notes on using the bag component
Components defined in the `validations` and `components` slots should not contain any children.

Ignore `v-slot="{ errors, valid }"` when defining `validation-provider` components in the `validations` slot because it is handled by the `bag` component. 

Ignore `v-model`, `:success`, and `:error-messages` when defining components in the `components` slots because they are handled by the `bag` component. 

## Scripting

Please see the [scripting](../Scripting.md#vueforms-module-statcanorchardcorevueforms) documentation.

## Workflow integration

Although this module does not require a workflow to handle the form submissions, we still provide a workflow hook to support performing some additional actions.

Please see the [workflow](../Workflows.md#vueforms-statcanorchardcorevueforms) documentation.

If you do use a workflow, you can use the `addError("name", "message");` script to add form errors. You can also use the `HttpRedirect` or `HttpResponse` workflow tasks to redirect or return a custom set of data to the client. All data returned by the HttpResponse task is available via the `form.responseData` object. You should also be able to use the `debug()` scripting method to return debugging data to the client.

## Localization (`StatCan.OrchardCore.VueForms.Localized`)

While you can use Orchard's [LocalizationPart](https://docs.orchardcore.net/en/dev/docs/reference/modules/ContentLocalization/#localizationpart) to localize your forms. We suggest you use the [LocalizedText](LocalizedText.md) feature to implement i18n in your forms. This part is what we weld to your VueForm content type when you enable this feature.

The `[locale]` shortcode is also useful to use in your views to localize simple text fields.

## Survey (`StatCan.OrchardCore.VueForms.Survey`)

Adds [SurveyJS](https://surveyjs.io/) support to VueForms.
This adds a part to the VueForm where you can specify the [SurveyJS creator](https://surveyjs.io/create-survey) json object.

In the Template, simply write this to output a Survey that uses the SurveyJS json object

```html
<survey :survey="survey"></survey>
```

To get the json output of the survey on your server side scripts, simply use the `getSurveyData()` method.

```javascript
const surveyData = getSurveyData();
debug("surveyData", surveyData);
```

## Examples

The following example forms are provided with the VueForms module as recipes:

- Contact Form
- UserProfile Form

### UserProfile Form

This form allows you to edit a `UserProfile` Content Type with the `CustomUserSettings` stereotype. For this form to work, you must enable the `Users Change Email` feature along with allowing users to update their email in the settings.
