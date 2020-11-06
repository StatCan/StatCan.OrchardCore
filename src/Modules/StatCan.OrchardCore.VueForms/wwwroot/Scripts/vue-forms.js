/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

"use strict"; // register VeeValidate components globally

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

Vue.component('validation-provider', VeeValidate.ValidationProvider);
Vue.component('validation-observer', VeeValidate.ValidationObserver); // include default english and french translations.

VeeValidate.localize({
  en: {
    "code": "en",
    "messages": {
      "alpha": "The {_field_} field may only contain alphabetic characters",
      "alpha_num": "The {_field_} field may only contain alpha-numeric characters",
      "alpha_dash": "The {_field_} field may contain alpha-numeric characters as well as dashes and underscores",
      "alpha_spaces": "The {_field_} field may only contain alphabetic characters as well as spaces",
      "between": "The {_field_} field must be between {min} and {max}",
      "confirmed": "The {_field_} field confirmation does not match",
      "digits": "The {_field_} field must be numeric and exactly contain {length} digits",
      "dimensions": "The {_field_} field must be {width} pixels by {height} pixels",
      "email": "The {_field_} field must be a valid email",
      "excluded": "The {_field_} field is not a valid value",
      "ext": "The {_field_} field is not a valid file",
      "image": "The {_field_} field must be an image",
      "integer": "The {_field_} field must be an integer",
      "length": "The {_field_} field must be {length} long",
      "max_value": "The {_field_} field must be {max} or less",
      "max": "The {_field_} field may not be greater than {length} characters",
      "mimes": "The {_field_} field must have a valid file type",
      "min_value": "The {_field_} field must be {min} or more",
      "min": "The {_field_} field must be at least {length} characters",
      "numeric": "The {_field_} field may only contain numeric characters",
      "oneOf": "The {_field_} field is not a valid value",
      "regex": "The {_field_} field format is invalid",
      "required_if": "The {_field_} field is required",
      "required": "The {_field_} field is required",
      "size": "The {_field_} field size must be less than {size}KB"
    }
  },
  fr: {
    "code": "fr",
    "messages": {
      "alpha": "Le champ {_field_} ne peut contenir que des lettres",
      "alpha_num": "Le champ {_field_} ne peut contenir que des caractères alpha-numériques",
      "alpha_dash": "Le champ {_field_} ne peut contenir que des caractères alpha-numériques, tirets ou soulignés",
      "alpha_spaces": "Le champ {_field_} ne peut contenir que des lettres ou des espaces",
      "between": "Le champ {_field_} doit être compris entre {min} et {max}",
      "confirmed": "Le champ {_field_} ne correspond pas",
      "digits": "Le champ {_field_} doit être un nombre entier de {length} chiffres",
      "dimensions": "Le champ {_field_} doit avoir une taille de {width} pixels par {height} pixels",
      "email": "Le champ {_field_} doit être une adresse e-mail valide",
      "excluded": "Le champ {_field_} doit être une valeur valide",
      "ext": "Le champ {_field_} doit être un fichier valide",
      "image": "Le champ {_field_} doit être une image",
      "integer": "Le champ {_field_} doit être un entier",
      "length": "Le champ {_field_} doit contenir {length} caractères",
      "max_value": "Le champ {_field_} doit avoir une valeur de {max} ou moins",
      "max": "Le champ {_field_} ne peut pas contenir plus de {length} caractères",
      "mimes": "Le champ {_field_} doit avoir un type MIME valide",
      "min_value": "Le champ {_field_} doit avoir une valeur de {min} ou plus",
      "min": "Le champ {_field_} doit contenir au minimum {length} caractères",
      "numeric": "Le champ {_field_} ne peut contenir que des chiffres",
      "oneOf": "Le champ {_field_} doit être une valeur valide",
      "regex": "Le champ {_field_} est invalide",
      "required": "Le champ {_field_} est obligatoire",
      "required_if": "Le champ {_field_} est obligatoire lorsque {target} possède cette valeur",
      "size": "Le champ {_field_} doit avoir un poids inférieur à {size}KB"
    }
  }
}); // run init script

function initForm(app) {
  // Set VeeValidate language based on the lang parameter
  VeeValidate.localize(app.dataset.lang); // run the vue-form init script provided in the OC admin ui

  var initScript = app.dataset.initScript;

  if (initScript) {
    var initFn = new Function(atob(initScript));
    initFn();
  }

  var modelScript = app.dataset.script;
  var parsedScript = {};

  if (modelScript) {
    var fn = new Function("return ".concat(atob(modelScript), ";"));
    parsedScript = fn();
  }

  var _parsedScript = parsedScript,
      parsedData = _parsedScript.data,
      parsedMethods = _parsedScript.methods,
      parsedRest = _objectWithoutProperties(_parsedScript, ["data", "methods"]);

  var objData = parsedData;

  if (typeof parsedData === "function") {
    objData = parsedData();
  }

  var defaultFormData = {
    submitting: false,
    submitSuccess: false,
    successMessage: undefined,
    submitError: false,
    errorMessage: undefined
  }; //todo component name

  Vue.component(app.dataset.name, _objectSpread(_objectSpread({}, parsedRest), {}, {
    template: "#".concat(app.dataset.name),
    data: function data() {
      return _objectSpread(_objectSpread({}, objData), {}, {
        form: _objectSpread({}, defaultFormData)
      });
    },
    methods: _objectSpread(_objectSpread({}, parsedMethods), {}, {
      formReset: function formReset() {
        Object.assign(this.$data.form, _objectSpread({}, defaultFormData)); // also reset the VeeValidate observer

        this.$refs.obs.reset();
      },
      formHandleSubmit: function formHandleSubmit(e) {
        console.log("test");
        e.preventDefault();
        var vm = this; // keep a reference to the VeeValidate observer

        var observer = vm.$refs.obs;
        observer.validate().then(function (valid) {
          if (valid) {
            var action = vm.$refs.form.getAttribute("action");
            var token = $("input[name='__RequestVerificationToken']").val();
            vm.form.submitting = true;
            $.ajax({
              type: "POST",
              url: action,
              data: _objectSpread(_objectSpread({}, vm.$data), {}, {
                __RequestVerificationToken: token
              }),
              cache: false,
              dataType: "json",
              success: function success(data) {
                Object.assign(vm.$data.form, _objectSpread({}, defaultFormData)); // if there are validation errors on the form, display them.

                if (data.validationError) {
                  vm.form.submitError = true;
                  vm.form.errorMessage = data.errorMessage;
                  observer.setErrors(data.errors);
                  return;
                } // if the server sends a redirect, reload the window


                if (data.redirect) {
                  window.location.href = data.redirect;
                  return;
                } //success, set the form success message 


                if (data.success) {
                  vm.form.submitSuccess = true;
                  vm.form.successMessage = data.successMessage;
                  return;
                }

                vm.form.submitError = true; // something went wrong, dev issue

                vm.form.errorMessage = "Something wen't wrong. Please report this to your site administrators. Error code: `VueForms.AjaxHandler`";
              },
              error: function error(xhr, statusText) {
                Object.assign(vm.$data.form, _objectSpread({}, defaultFormData));
                vm.form.submitError = true;
                vm.form.errorMessage = "".concat(xhr.status, " ").concat(statusText);
              }
            });
          }
        });
        return false;
      }
    })
  }));
  new Vue({
    el: app,
    vuetify: new Vuetify()
  });
}

document.addEventListener("DOMContentLoaded", function (event) {
  // look for all vue forms when this script is loaded and initialize them
  document.querySelectorAll(".vue-form").forEach(initForm);
});