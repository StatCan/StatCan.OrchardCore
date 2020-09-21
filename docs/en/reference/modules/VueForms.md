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

It is also possible to modify the vuetify [global-config](https://vuetifyjs.com/en/customization/global-config/) or [presets](https://vuetifyjs.com/en/customization/presets/). All you have to do is return the Vuetify instance from the init script.


```javascript
VeeValidate.setInteractionMode('passive');

return new Vuetify({
  theme: {
    dark: true,
  },
});
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

### VueComponent Widget

The VueComponent widget is meant to be used inside a VueForm and allows a user to write a vuejs component with a custom name, template and script. 

#### Props

The VueJS component you are writing using this widget has access to some of the properties of the parent VueForm model and the VeeValidate [ValidationProvider](https://logaretm.github.io/vee-validate/api/validation-observer.html) props.
The props prefixed with obs come from the `ValidationProvider` and the ones prefixed with form come from our custom implementation.

Then accessing these properties in your template / script. You need to change the hyphens to camel case. `obs-valid` becomes `obsValid`

| Property Name  | Definition |
|--------------- |------------|
| obs-valid | True if all fields are valid. |
| obs-invalid| True if at least one field is invalid. |
| obs-reset | A method that resets validation state for all providers. `() => void` |
| obs-validate | A method that triggers validation for all providers. Mutates child providers state unless silent is true. `() => Promise<boolean>` |
| form-handle-submit | A method that calls the `validate()` method and then, if valid, sends an ajax request to our controller `() => void` |
| form-success-message | The success message returned from the server as specified in the [VueForm](#vueform-part) |
| form-error-message | The error message returned from the server as specified in the [VueForm](#vueform-part) |
| form-ajax-error-status | Returns an unsigned short with the status of the response of the request. |
| form-ajax-error-text | Returns a DOMString containing the response string returned by the HTTP server. This includes the entire text of the response message ("200 OK", for example). |

#### Title 

The title of the VueComponent widget is used as the VueJS Component name when generating the component. Please adhere to [these ](https://vuejs.org/v2/style-guide/#Multi-word-component-names-essential) VueJS component naming recommendation. Note that we [slugify](https://docs.orchardcore.net/en/dev/docs/reference/modules/Liquid/#slugify) the title to remove unwanted spaces. 

#### Template

The template is where you write the VueJS component template.

Important implementation notes:
- This field should return a **single** vue / html node.
- Make sure you add a name to all your inputs. The name should match the v-model name as we use this to map the server side errors to your inputs.

```html
<v-container style="max-width: 800px">
  <v-card class="elevation-24">
    <v-toolbar dark color="primary">
      <v-toolbar-title>My Form</v-toolbar-title>
    </v-toolbar>
    <v-card-text>
      <v-alert type="success" v-if="formSuccessMessage">
        {% raw %}{{formSuccessMessage}}{% endraw %}
      </v-alert>
      <v-alert type="error" v-if="formErrorMessage">
        {% raw %}{{formErrorMessage}}{% endraw %}
      </v-alert>
      <validation-provider name="name" rules="" v-slot="{ errors, valid }">
        <v-text-field
			name="name"
			v-model="name"
            :counter="10"
            :error-messages="errors"
            label="name"
                      ></v-text-field>
      </validation-provider>
      <validation-provider name="email" rules="required|email"  v-slot="{ errors, valid }">
        <v-text-field
            name="email"
            v-model="email"
            :error-messages="errors"
            :success="valid"
            label="E-mail"
            required
		></v-text-field>
      </validation-provider>

      <validation-provider name="gender" rules="required" v-slot="{ errors, valid }">
        <v-select
			name="gender"
            :items="items"
            v-model="select"
            :error-messages="errors"
            :success="valid"
            label="Gender"
            required
        ></v-select>
      </validation-provider>
    </v-card-text>
    <v-card-actions>
      <v-btn @click="obsReset">Clear</v-btn>
      <v-spacer></v-spacer>
      <v-btn @click="obsValidate()">Validate</v-btn>
      <v-btn color="primary" @click="formHandleSubmit">Sign Up</v-btn>
    </v-card-actions>
  </v-card>
</v-container>
```

#### Script

The script is where you write the[VueJS component options object](https://012.vuejs.org/api/options.html). Our custom implementation will **override** the [prop](https://012.vuejs.org/api/options.html#props) and [template](https://012.vuejs.org/api/options.html#template) options you add to the object. 

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

## Scripting

Please see the [scripting](../Scripting.md#vueforms-module-statcanorchardcorevueforms) documentation.

## Workflow integration

Although this module does not require a workflow to handle the form submissions, we still provide a workflow hook to support performing some additional actions.

Please see the [workflow](../Workflows.md#vueforms-statcanorchardcorevueforms) documentation.

## Localization (`StatCan.OrchardCore.VueForms.Localized`)

While you can use Orchard's [LocalizationPart](https://docs.orchardcore.net/en/dev/docs/reference/modules/ContentLocalization/#localizationpart) to localize your forms. We suggest you use the [LocalizedText](LocalizedText.md) feature to implement i18n in your forms. This part is what we weld to your VueForm content type when you enable this feature.