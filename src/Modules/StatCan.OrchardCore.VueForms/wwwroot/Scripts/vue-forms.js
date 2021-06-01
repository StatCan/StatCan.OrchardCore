/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

// register VeeValidate components globally
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
});
"use strict";

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it.return != null) it.return(); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

function _typeof(obj) { "@babel/helpers - typeof"; if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function ownKeys(object, enumerableOnly) { var keys = Object.keys(object); if (Object.getOwnPropertySymbols) { var symbols = Object.getOwnPropertySymbols(object); if (enumerableOnly) { symbols = symbols.filter(function (sym) { return Object.getOwnPropertyDescriptor(object, sym).enumerable; }); } keys.push.apply(keys, symbols); } return keys; }

function _objectSpread(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; if (i % 2) { ownKeys(Object(source), true).forEach(function (key) { _defineProperty(target, key, source[key]); }); } else if (Object.getOwnPropertyDescriptors) { Object.defineProperties(target, Object.getOwnPropertyDescriptors(source)); } else { ownKeys(Object(source)).forEach(function (key) { Object.defineProperty(target, key, Object.getOwnPropertyDescriptor(source, key)); }); } } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = _objectWithoutPropertiesLoose(source, excluded); var key, i; if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function decodeUnicode(str) {
  return decodeURIComponent(atob(str).split("").map(function (c) {
    return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(""));
} // run init script


function initForm(app) {
  // Set VeeValidate language based on the lang parameter
  VeeValidate.localize(app.dataset.lang);
  var componentOptions = app.dataset.options;
  var parsedOptions = {};

  if (componentOptions) {
    var fn = new Function("return (".concat(decodeUnicode(componentOptions), ");"));
    parsedOptions = fn();

    if (!parsedOptions) {
      console.log("Could not parse the componentOptions object. Make sure your object is well formed.");
    }
  }

  var _parsedOptions = parsedOptions,
      parsedData = _parsedOptions.data,
      parsedMethods = _parsedOptions.methods,
      parsedRest = _objectWithoutProperties(_parsedOptions, ["data", "methods"]);

  var objData = parsedData;

  if (typeof parsedData === "function") {
    objData = parsedData();
  }

  var defaultFormData = {
    submitting: false,
    submitSuccess: false,
    successMessage: undefined,
    submitError: false,
    submitValidationErrors: false,
    serverValidationMessage: undefined,
    serverErrorMessage: undefined,
    responseData: undefined
  };
  Vue.component(app.dataset.name, function (resolve) {
    resolve(_objectSpread(_objectSpread({}, parsedRest), {}, {
      template: "#".concat(app.dataset.name),
      data: function data() {
        return _objectSpread(_objectSpread({}, objData), {}, {
          form: _objectSpread({}, defaultFormData)
        });
      },
      methods: _objectSpread(_objectSpread({
        // default method that return the data to be submitted to the server
        // this was added first to allow the Administrator to edit this function on the OC Admin
        submitData: function submitData() {
          return _objectSpread({}, this.$data);
        }
      }, parsedMethods), {}, {
        formReset: function formReset() {
          this.form = _objectSpread({}, defaultFormData); // also reset the VeeValidate observer

          this.$refs.obs.reset();
        },
        formHandleSubmit: function formHandleSubmit(e) {
          var _this = this;

          e.preventDefault();
          var vm = this; // keep a reference to the VeeValidate observer

          var observer = vm.$refs.obs;
          observer.validate().then(function (valid) {
            if (valid) {
              var action = vm.$refs.form.getAttribute("action"); // set form vue data

              vm.form.submitting = true;
              var formData = new FormData();
              formData.append("__RequestVerificationToken", vm.$refs.form.querySelector('input[name="__RequestVerificationToken"]').value);

              if ((typeof grecaptcha === "undefined" ? "undefined" : _typeof(grecaptcha)) == "object") {
                formData.append("recaptcha", grecaptcha.getResponse());
              }

              var submitData = vm.submitData();

              for (var key in submitData) {
                formData.append(key, submitData[key]);
              } // iterate all file inputs and add the files to the request


              $(_this.$refs.form).find("input[type=file]").each(function () {
                var _iterator = _createForOfIteratorHelper(this.files),
                    _step;

                try {
                  for (_iterator.s(); !(_step = _iterator.n()).done;) {
                    var file = _step.value;
                    formData.append(file.name, file);
                  }
                } catch (err) {
                  _iterator.e(err);
                } finally {
                  _iterator.f();
                }
              });
              $.ajax({
                type: "POST",
                url: action,
                data: formData,
                cache: false,
                dataType: "json",
                // expect json from the server
                processData: false,
                //tell jquery not to process data
                contentType: false,
                //tell jquery not to set content-type
                success: function success(data) {
                  vm.form = _objectSpread({}, defaultFormData);
                  vm.form.responseData = data; // if there are validation errors on the form, display them.

                  if (data.validationError) {
                    //legacy
                    if (data.errors["serverValidationMessage"] != null) {
                      vm.form.serverValidationMessage = data.errors["serverValidationMessage"];
                    }

                    vm.form.submitValidationError = true;
                    observer.setErrors(data.errors);
                    return;
                  } // if the server sends a redirect, reload the window


                  if (data.redirect) {
                    window.location.href = data.redirect;
                    return;
                  }

                  vm.form.submitSuccess = true;
                  vm.form.successMessage = data.successMessage;
                  return;
                },
                error: function error(xhr, statusText) {
                  vm.form = _objectSpread({}, defaultFormData);
                  vm.form.submitError = true;
                  vm.form.serverErrorMessage = "".concat(xhr.status, " ").concat(statusText);
                }
              });
            }
          });
          return false;
        }
      })
    }));
  }); // run the vue-form init script provided in the OC admin ui

  var initScript = app.dataset.initScript;

  if (initScript) {
    var initFn = new Function(decodeUnicode(initScript));
    initFn();
  }
} // look for all vue forms when this script is loaded and initialize them


document.querySelectorAll(".vue-form").forEach(initForm);
document.addEventListener("DOMContentLoaded", function (event) {
  document.querySelectorAll(".vue-app-instance").forEach(function (elem) {
    new Vue({
      el: elem
    });
  });
  document.querySelectorAll(".vuetify-app-instance").forEach(function (elem) {
    new Vue({
      el: elem,
      vuetify: new Vuetify()
    });
  });
});
//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbInZlZXZhbGlkYXRlLXNldHVwLmpzIiwidnVlLWZvcm1zLmpzIl0sIm5hbWVzIjpbIlZ1ZSIsImNvbXBvbmVudCIsIlZlZVZhbGlkYXRlIiwiVmFsaWRhdGlvblByb3ZpZGVyIiwiVmFsaWRhdGlvbk9ic2VydmVyIiwibG9jYWxpemUiLCJlbiIsImZyIiwiZGVjb2RlVW5pY29kZSIsInN0ciIsImRlY29kZVVSSUNvbXBvbmVudCIsImF0b2IiLCJzcGxpdCIsIm1hcCIsImMiLCJjaGFyQ29kZUF0IiwidG9TdHJpbmciLCJzbGljZSIsImpvaW4iLCJpbml0Rm9ybSIsImFwcCIsImRhdGFzZXQiLCJsYW5nIiwiY29tcG9uZW50T3B0aW9ucyIsIm9wdGlvbnMiLCJwYXJzZWRPcHRpb25zIiwiZm4iLCJGdW5jdGlvbiIsImNvbnNvbGUiLCJsb2ciLCJwYXJzZWREYXRhIiwiZGF0YSIsInBhcnNlZE1ldGhvZHMiLCJtZXRob2RzIiwicGFyc2VkUmVzdCIsIm9iakRhdGEiLCJkZWZhdWx0Rm9ybURhdGEiLCJzdWJtaXR0aW5nIiwic3VibWl0U3VjY2VzcyIsInN1Y2Nlc3NNZXNzYWdlIiwidW5kZWZpbmVkIiwic3VibWl0RXJyb3IiLCJzdWJtaXRWYWxpZGF0aW9uRXJyb3JzIiwic2VydmVyVmFsaWRhdGlvbk1lc3NhZ2UiLCJzZXJ2ZXJFcnJvck1lc3NhZ2UiLCJyZXNwb25zZURhdGEiLCJuYW1lIiwicmVzb2x2ZSIsInRlbXBsYXRlIiwiZm9ybSIsInN1Ym1pdERhdGEiLCIkZGF0YSIsImZvcm1SZXNldCIsIiRyZWZzIiwib2JzIiwicmVzZXQiLCJmb3JtSGFuZGxlU3VibWl0IiwiZSIsInByZXZlbnREZWZhdWx0Iiwidm0iLCJvYnNlcnZlciIsInZhbGlkYXRlIiwidGhlbiIsInZhbGlkIiwiYWN0aW9uIiwiZ2V0QXR0cmlidXRlIiwiZm9ybURhdGEiLCJGb3JtRGF0YSIsImFwcGVuZCIsInF1ZXJ5U2VsZWN0b3IiLCJ2YWx1ZSIsImdyZWNhcHRjaGEiLCJnZXRSZXNwb25zZSIsImtleSIsIiQiLCJmaW5kIiwiZWFjaCIsImZpbGVzIiwiZmlsZSIsImFqYXgiLCJ0eXBlIiwidXJsIiwiY2FjaGUiLCJkYXRhVHlwZSIsInByb2Nlc3NEYXRhIiwiY29udGVudFR5cGUiLCJzdWNjZXNzIiwidmFsaWRhdGlvbkVycm9yIiwiZXJyb3JzIiwic3VibWl0VmFsaWRhdGlvbkVycm9yIiwic2V0RXJyb3JzIiwicmVkaXJlY3QiLCJ3aW5kb3ciLCJsb2NhdGlvbiIsImhyZWYiLCJlcnJvciIsInhociIsInN0YXR1c1RleHQiLCJzdGF0dXMiLCJpbml0U2NyaXB0IiwiaW5pdEZuIiwiZG9jdW1lbnQiLCJxdWVyeVNlbGVjdG9yQWxsIiwiZm9yRWFjaCIsImFkZEV2ZW50TGlzdGVuZXIiLCJldmVudCIsImVsZW0iLCJlbCIsInZ1ZXRpZnkiLCJWdWV0aWZ5Il0sIm1hcHBpbmdzIjoiOzs7OztBQUNBO0FBQ0FBLEdBQUcsQ0FBQ0MsU0FBSixDQUFjLHFCQUFkLEVBQXFDQyxXQUFXLENBQUNDLGtCQUFqRDtBQUNBSCxHQUFHLENBQUNDLFNBQUosQ0FBYyxxQkFBZCxFQUFxQ0MsV0FBVyxDQUFDRSxrQkFBakQsRSxDQUVBOztBQUNBRixXQUFXLENBQUNHLFFBQVosQ0FBcUI7QUFDbkJDLEVBQUFBLEVBQUUsRUFBQztBQUNELFlBQVEsSUFEUDtBQUVELGdCQUFZO0FBQ1YsZUFBUyw0REFEQztBQUVWLG1CQUFhLCtEQUZIO0FBR1Ysb0JBQWMsNEZBSEo7QUFJVixzQkFBZ0IsOEVBSk47QUFLVixpQkFBVyxxREFMRDtBQU1WLG1CQUFhLGlEQU5IO0FBT1YsZ0JBQVUseUVBUEE7QUFRVixvQkFBYywrREFSSjtBQVNWLGVBQVMsMkNBVEM7QUFVVixrQkFBWSwwQ0FWRjtBQVdWLGFBQU8seUNBWEc7QUFZVixlQUFTLHNDQVpDO0FBYVYsaUJBQVcsd0NBYkQ7QUFjVixnQkFBVSwyQ0FkQTtBQWVWLG1CQUFhLDJDQWZIO0FBZ0JWLGFBQU8saUVBaEJHO0FBaUJWLGVBQVMsaURBakJDO0FBa0JW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ZmlsZSI6InZ1ZS1mb3Jtcy5qcyIsInNvdXJjZXNDb250ZW50IjpbIlxyXG4vLyByZWdpc3RlciBWZWVWYWxpZGF0ZSBjb21wb25lbnRzIGdsb2JhbGx5XHJcblZ1ZS5jb21wb25lbnQoJ3ZhbGlkYXRpb24tcHJvdmlkZXInLCBWZWVWYWxpZGF0ZS5WYWxpZGF0aW9uUHJvdmlkZXIpO1xyXG5WdWUuY29tcG9uZW50KCd2YWxpZGF0aW9uLW9ic2VydmVyJywgVmVlVmFsaWRhdGUuVmFsaWRhdGlvbk9ic2VydmVyKTsgXHJcblxyXG4vLyBpbmNsdWRlIGRlZmF1bHQgZW5nbGlzaCBhbmQgZnJlbmNoIHRyYW5zbGF0aW9ucy5cclxuVmVlVmFsaWRhdGUubG9jYWxpemUoe1xyXG4gIGVuOntcclxuICAgIFwiY29kZVwiOiBcImVuXCIsXHJcbiAgICBcIm1lc3NhZ2VzXCI6IHtcclxuICAgICAgXCJhbHBoYVwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbWF5IG9ubHkgY29udGFpbiBhbHBoYWJldGljIGNoYXJhY3RlcnNcIixcclxuICAgICAgXCJhbHBoYV9udW1cIjogXCJUaGUge19maWVsZF99IGZpZWxkIG1heSBvbmx5IGNvbnRhaW4gYWxwaGEtbnVtZXJpYyBjaGFyYWN0ZXJzXCIsXHJcbiAgICAgIFwiYWxwaGFfZGFzaFwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbWF5IGNvbnRhaW4gYWxwaGEtbnVtZXJpYyBjaGFyYWN0ZXJzIGFzIHdlbGwgYXMgZGFzaGVzIGFuZCB1bmRlcnNjb3Jlc1wiLFxyXG4gICAgICBcImFscGhhX3NwYWNlc1wiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbWF5IG9ubHkgY29udGFpbiBhbHBoYWJldGljIGNoYXJhY3RlcnMgYXMgd2VsbCBhcyBzcGFjZXNcIixcclxuICAgICAgXCJiZXR3ZWVuXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBtdXN0IGJlIGJldHdlZW4ge21pbn0gYW5kIHttYXh9XCIsXHJcbiAgICAgIFwiY29uZmlybWVkXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBjb25maXJtYXRpb24gZG9lcyBub3QgbWF0Y2hcIixcclxuICAgICAgXCJkaWdpdHNcIjogXCJUaGUge19maWVsZF99IGZpZWxkIG11c3QgYmUgbnVtZXJpYyBhbmQgZXhhY3RseSBjb250YWluIHtsZW5ndGh9IGRpZ2l0c1wiLFxyXG4gICAgICBcImRpbWVuc2lvbnNcIjogXCJUaGUge19maWVsZF99IGZpZWxkIG11c3QgYmUge3dpZHRofSBwaXhlbHMgYnkge2hlaWdodH0gcGl4ZWxzXCIsXHJcbiAgICAgIFwiZW1haWxcIjogXCJUaGUge19maWVsZF99IGZpZWxkIG11c3QgYmUgYSB2YWxpZCBlbWFpbFwiLFxyXG4gICAgICBcImV4Y2x1ZGVkXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBpcyBub3QgYSB2YWxpZCB2YWx1ZVwiLFxyXG4gICAgICBcImV4dFwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgaXMgbm90IGEgdmFsaWQgZmlsZVwiLFxyXG4gICAgICBcImltYWdlXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBtdXN0IGJlIGFuIGltYWdlXCIsXHJcbiAgICAgIFwiaW50ZWdlclwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbXVzdCBiZSBhbiBpbnRlZ2VyXCIsXHJcbiAgICAgIFwibGVuZ3RoXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBtdXN0IGJlIHtsZW5ndGh9IGxvbmdcIixcclxuICAgICAgXCJtYXhfdmFsdWVcIjogXCJUaGUge19maWVsZF99IGZpZWxkIG11c3QgYmUge21heH0gb3IgbGVzc1wiLFxyXG4gICAgICBcIm1heFwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbWF5IG5vdCBiZSBncmVhdGVyIHRoYW4ge2xlbmd0aH0gY2hhcmFjdGVyc1wiLFxyXG4gICAgICBcIm1pbWVzXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBtdXN0IGhhdmUgYSB2YWxpZCBmaWxlIHR5cGVcIixcclxuICAgICAgXCJtaW5fdmFsdWVcIjogXCJUaGUge19maWVsZF99IGZpZWxkIG11c3QgYmUge21pbn0gb3IgbW9yZVwiLFxyXG4gICAgICBcIm1pblwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbXVzdCBiZSBhdCBsZWFzdCB7bGVuZ3RofSBjaGFyYWN0ZXJzXCIsXHJcbiAgICAgIFwibnVtZXJpY1wiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgbWF5IG9ubHkgY29udGFpbiBudW1lcmljIGNoYXJhY3RlcnNcIixcclxuICAgICAgXCJvbmVPZlwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgaXMgbm90IGEgdmFsaWQgdmFsdWVcIixcclxuICAgICAgXCJyZWdleFwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgZm9ybWF0IGlzIGludmFsaWRcIixcclxuICAgICAgXCJyZXF1aXJlZF9pZlwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgaXMgcmVxdWlyZWRcIixcclxuICAgICAgXCJyZXF1aXJlZFwiOiBcIlRoZSB7X2ZpZWxkX30gZmllbGQgaXMgcmVxdWlyZWRcIixcclxuICAgICAgXCJzaXplXCI6IFwiVGhlIHtfZmllbGRffSBmaWVsZCBzaXplIG11c3QgYmUgbGVzcyB0aGFuIHtzaXplfUtCXCJcclxuICAgIH1cclxuICB9LFxyXG4gIGZyOiB7XHJcbiAgICBcImNvZGVcIjogXCJmclwiLFxyXG4gICAgXCJtZXNzYWdlc1wiOiB7XHJcbiAgICAgIFwiYWxwaGFcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gbmUgcGV1dCBjb250ZW5pciBxdWUgZGVzIGxldHRyZXNcIixcclxuICAgICAgXCJhbHBoYV9udW1cIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gbmUgcGV1dCBjb250ZW5pciBxdWUgZGVzIGNhcmFjdMOocmVzIGFscGhhLW51bcOpcmlxdWVzXCIsXHJcbiAgICAgIFwiYWxwaGFfZGFzaFwiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBuZSBwZXV0IGNvbnRlbmlyIHF1ZSBkZXMgY2FyYWN0w6hyZXMgYWxwaGEtbnVtw6lyaXF1ZXMsIHRpcmV0cyBvdSBzb3VsaWduw6lzXCIsXHJcbiAgICAgIFwiYWxwaGFfc3BhY2VzXCI6IFwiTGUgY2hhbXAge19maWVsZF99IG5lIHBldXQgY29udGVuaXIgcXVlIGRlcyBsZXR0cmVzIG91IGRlcyBlc3BhY2VzXCIsXHJcbiAgICAgIFwiYmV0d2VlblwiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBkb2l0IMOqdHJlIGNvbXByaXMgZW50cmUge21pbn0gZXQge21heH1cIixcclxuICAgICAgXCJjb25maXJtZWRcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gbmUgY29ycmVzcG9uZCBwYXNcIixcclxuICAgICAgXCJkaWdpdHNcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gZG9pdCDDqnRyZSB1biBub21icmUgZW50aWVyIGRlIHtsZW5ndGh9IGNoaWZmcmVzXCIsXHJcbiAgICAgIFwiZGltZW5zaW9uc1wiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBkb2l0IGF2b2lyIHVuZSB0YWlsbGUgZGUge3dpZHRofSBwaXhlbHMgcGFyIHtoZWlnaHR9IHBpeGVsc1wiLFxyXG4gICAgICBcImVtYWlsXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgw6p0cmUgdW5lIGFkcmVzc2UgZS1tYWlsIHZhbGlkZVwiLFxyXG4gICAgICBcImV4Y2x1ZGVkXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgw6p0cmUgdW5lIHZhbGV1ciB2YWxpZGVcIixcclxuICAgICAgXCJleHRcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gZG9pdCDDqnRyZSB1biBmaWNoaWVyIHZhbGlkZVwiLFxyXG4gICAgICBcImltYWdlXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgw6p0cmUgdW5lIGltYWdlXCIsXHJcbiAgICAgIFwiaW50ZWdlclwiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBkb2l0IMOqdHJlIHVuIGVudGllclwiLFxyXG4gICAgICBcImxlbmd0aFwiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBkb2l0IGNvbnRlbmlyIHtsZW5ndGh9IGNhcmFjdMOocmVzXCIsXHJcbiAgICAgIFwibWF4X3ZhbHVlXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgYXZvaXIgdW5lIHZhbGV1ciBkZSB7bWF4fSBvdSBtb2luc1wiLFxyXG4gICAgICBcIm1heFwiOiBcIkxlIGNoYW1wIHtfZmllbGRffSBuZSBwZXV0IHBhcyBjb250ZW5pciBwbHVzIGRlIHtsZW5ndGh9IGNhcmFjdMOocmVzXCIsXHJcbiAgICAgIFwibWltZXNcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gZG9pdCBhdm9pciB1biB0eXBlIE1JTUUgdmFsaWRlXCIsXHJcbiAgICAgIFwibWluX3ZhbHVlXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgYXZvaXIgdW5lIHZhbGV1ciBkZSB7bWlufSBvdSBwbHVzXCIsXHJcbiAgICAgIFwibWluXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGRvaXQgY29udGVuaXIgYXUgbWluaW11bSB7bGVuZ3RofSBjYXJhY3TDqHJlc1wiLFxyXG4gICAgICBcIm51bWVyaWNcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gbmUgcGV1dCBjb250ZW5pciBxdWUgZGVzIGNoaWZmcmVzXCIsXHJcbiAgICAgIFwib25lT2ZcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gZG9pdCDDqnRyZSB1bmUgdmFsZXVyIHZhbGlkZVwiLFxyXG4gICAgICBcInJlZ2V4XCI6IFwiTGUgY2hhbXAge19maWVsZF99IGVzdCBpbnZhbGlkZVwiLFxyXG4gICAgICBcInJlcXVpcmVkXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGVzdCBvYmxpZ2F0b2lyZVwiLFxyXG4gICAgICBcInJlcXVpcmVkX2lmXCI6IFwiTGUgY2hhbXAge19maWVsZF99IGVzdCBvYmxpZ2F0b2lyZSBsb3JzcXVlIHt0YXJnZXR9IHBvc3PDqGRlIGNldHRlIHZhbGV1clwiLFxyXG4gICAgICBcInNpemVcIjogXCJMZSBjaGFtcCB7X2ZpZWxkX30gZG9pdCBhdm9pciB1biBwb2lkcyBpbmbDqXJpZXVyIMOgIHtzaXplfUtCXCJcclxuICAgIH1cclxuICB9LFxyXG59KTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XG5cbmZ1bmN0aW9uIGRlY29kZVVuaWNvZGUoc3RyKSB7XG4gIHJldHVybiBkZWNvZGVVUklDb21wb25lbnQoXG4gICAgYXRvYihzdHIpXG4gICAgICAuc3BsaXQoXCJcIilcbiAgICAgIC5tYXAoZnVuY3Rpb24gKGMpIHtcbiAgICAgICAgcmV0dXJuIFwiJVwiICsgKFwiMDBcIiArIGMuY2hhckNvZGVBdCgwKS50b1N0cmluZygxNikpLnNsaWNlKC0yKTtcbiAgICAgIH0pXG4gICAgICAuam9pbihcIlwiKVxuICApO1xufVxuXG4vLyBydW4gaW5pdCBzY3JpcHRcbmZ1bmN0aW9uIGluaXRGb3JtKGFwcCkge1xuICAvLyBTZXQgVmVlVmFsaWRhdGUgbGFuZ3VhZ2UgYmFzZWQgb24gdGhlIGxhbmcgcGFyYW1ldGVyXG4gIFZlZVZhbGlkYXRlLmxvY2FsaXplKGFwcC5kYXRhc2V0LmxhbmcpO1xuXG4gIGxldCBjb21wb25lbnRPcHRpb25zID0gYXBwLmRhdGFzZXQub3B0aW9ucztcblxuICBsZXQgcGFyc2VkT3B0aW9ucyA9IHt9O1xuICBpZiAoY29tcG9uZW50T3B0aW9ucykge1xuICAgIGNvbnN0IGZuID0gbmV3IEZ1bmN0aW9uKGByZXR1cm4gKCR7ZGVjb2RlVW5pY29kZShjb21wb25lbnRPcHRpb25zKX0pO2ApO1xuICAgIHBhcnNlZE9wdGlvbnMgPSBmbigpO1xuICAgIGlmICghcGFyc2VkT3B0aW9ucykge1xuICAgICAgY29uc29sZS5sb2coXG4gICAgICAgIFwiQ291bGQgbm90IHBhcnNlIHRoZSBjb21wb25lbnRPcHRpb25zIG9iamVjdC4gTWFrZSBzdXJlIHlvdXIgb2JqZWN0IGlzIHdlbGwgZm9ybWVkLlwiXG4gICAgICApO1xuICAgIH1cbiAgfVxuXG4gIGNvbnN0IHtcbiAgICBkYXRhOiBwYXJzZWREYXRhLFxuICAgIG1ldGhvZHM6IHBhcnNlZE1ldGhvZHMsXG4gICAgLi4ucGFyc2VkUmVzdFxuICB9ID0gcGFyc2VkT3B0aW9ucztcbiAgbGV0IG9iakRhdGEgPSBwYXJzZWREYXRhO1xuICBpZiAodHlwZW9mIHBhcnNlZERhdGEgPT09IFwiZnVuY3Rpb25cIikge1xuICAgIG9iakRhdGEgPSBwYXJzZWREYXRhKCk7XG4gIH1cbiAgY29uc3QgZGVmYXVsdEZvcm1EYXRhID0ge1xuICAgIHN1Ym1pdHRpbmc6IGZhbHNlLFxuICAgIHN1Ym1pdFN1Y2Nlc3M6IGZhbHNlLFxuICAgIHN1Y2Nlc3NNZXNzYWdlOiB1bmRlZmluZWQsXG4gICAgc3VibWl0RXJyb3I6IGZhbHNlLFxuICAgIHN1Ym1pdFZhbGlkYXRpb25FcnJvcnM6IGZhbHNlLFxuICAgIHNlcnZlclZhbGlkYXRpb25NZXNzYWdlOiB1bmRlZmluZWQsXG4gICAgc2VydmVyRXJyb3JNZXNzYWdlOiB1bmRlZmluZWQsXG4gICAgcmVzcG9uc2VEYXRhOiB1bmRlZmluZWQsXG4gIH07XG5cbiAgVnVlLmNvbXBvbmVudChhcHAuZGF0YXNldC5uYW1lLCBmdW5jdGlvbiAocmVzb2x2ZSkge1xuICAgIHJlc29sdmUoe1xuICAgICAgLy8gRmlyc3QgYmVjYXVzZSB0aGUgZWxlbWVudHMgYmVsb3cgd2lsbCBvdmVycmlkZVxuICAgICAgLi4ucGFyc2VkUmVzdCxcbiAgICAgIHRlbXBsYXRlOiBgIyR7YXBwLmRhdGFzZXQubmFtZX1gLFxuICAgICAgZGF0YTogZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4ge1xuICAgICAgICAgIC4uLm9iakRhdGEsXG4gICAgICAgICAgZm9ybTogeyAuLi5kZWZhdWx0Rm9ybURhdGEgfSxcbiAgICAgICAgfTtcbiAgICAgIH0sXG4gICAgICBtZXRob2RzOiB7XG4gICAgICAgIC8vIGRlZmF1bHQgbWV0aG9kIHRoYXQgcmV0dXJuIHRoZSBkYXRhIHRvIGJlIHN1Ym1pdHRlZCB0byB0aGUgc2VydmVyXG4gICAgICAgIC8vIHRoaXMgd2FzIGFkZGVkIGZpcnN0IHRvIGFsbG93IHRoZSBBZG1pbmlzdHJhdG9yIHRvIGVkaXQgdGhpcyBmdW5jdGlvbiBvbiB0aGUgT0MgQWRtaW5cbiAgICAgICAgc3VibWl0RGF0YSgpIHtcbiAgICAgICAgICByZXR1cm4geyAuLi50aGlzLiRkYXRhIH07XG4gICAgICAgIH0sXG4gICAgICAgIC4uLnBhcnNlZE1ldGhvZHMsXG4gICAgICAgIGZvcm1SZXNldCgpIHtcbiAgICAgICAgICB0aGlzLmZvcm0gPSB7IC4uLmRlZmF1bHRGb3JtRGF0YSB9O1xuICAgICAgICAgIC8vIGFsc28gcmVzZXQgdGhlIFZlZVZhbGlkYXRlIG9ic2VydmVyXG4gICAgICAgICAgdGhpcy4kcmVmcy5vYnMucmVzZXQoKTtcbiAgICAgICAgfSxcbiAgICAgICAgZm9ybUhhbmRsZVN1Ym1pdChlKSB7XG4gICAgICAgICAgZS5wcmV2ZW50RGVmYXVsdCgpO1xuICAgICAgICAgIGNvbnN0IHZtID0gdGhpcztcbiAgICAgICAgICAvLyBrZWVwIGEgcmVmZXJlbmNlIHRvIHRoZSBWZWVWYWxpZGF0ZSBvYnNlcnZlclxuICAgICAgICAgIGNvbnN0IG9ic2VydmVyID0gdm0uJHJlZnMub2JzO1xuICAgICAgICAgIG9ic2VydmVyLnZhbGlkYXRlKCkudGhlbigodmFsaWQpID0+IHtcbiAgICAgICAgICAgIGlmICh2YWxpZCkge1xuICAgICAgICAgICAgICBjb25zdCBhY3Rpb24gPSB2bS4kcmVmcy5mb3JtLmdldEF0dHJpYnV0ZShcImFjdGlvblwiKTtcbiAgICAgICAgICAgICAgXG4gICAgICAgICAgICAgIC8vIHNldCBmb3JtIHZ1ZSBkYXRhXG4gICAgICAgICAgICAgIHZtLmZvcm0uc3VibWl0dGluZyA9IHRydWU7XG5cbiAgICAgICAgICAgICAgbGV0IGZvcm1EYXRhID0gbmV3IEZvcm1EYXRhKCk7XG4gICAgICAgICAgICAgIGZvcm1EYXRhLmFwcGVuZChcIl9fUmVxdWVzdFZlcmlmaWNhdGlvblRva2VuXCIsIHZtLiRyZWZzLmZvcm0ucXVlcnlTZWxlY3RvcihcbiAgICAgICAgICAgICAgICAnaW5wdXRbbmFtZT1cIl9fUmVxdWVzdFZlcmlmaWNhdGlvblRva2VuXCJdJ1xuICAgICAgICAgICAgICApLnZhbHVlKVxuICAgICAgICAgICAgICBpZiAodHlwZW9mIGdyZWNhcHRjaGEgPT0gXCJvYmplY3RcIikge1xuICAgICAgICAgICAgICAgIGZvcm1EYXRhLmFwcGVuZChcInJlY2FwdGNoYVwiLCBncmVjYXB0Y2hhLmdldFJlc3BvbnNlKCkpO1xuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgXG4gICAgICAgICAgICAgIGxldCBzdWJtaXREYXRhID0gdm0uc3VibWl0RGF0YSgpO1xuICAgICAgICAgICAgICBmb3IgKGNvbnN0IGtleSBpbiBzdWJtaXREYXRhKSB7XG4gICAgICAgICAgICAgICAgZm9ybURhdGEuYXBwZW5kKGtleSwgc3VibWl0RGF0YVtrZXldKTtcbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICBcbiAgICAgICAgICAgICAgLy8gaXRlcmF0ZSBhbGwgZmlsZSBpbnB1dHMgYW5kIGFkZCB0aGUgZmlsZXMgdG8gdGhlIHJlcXVlc3RcbiAgICAgICAgICAgICAgJCh0aGlzLiRyZWZzLmZvcm0pLmZpbmQoXCJpbnB1dFt0eXBlPWZpbGVdXCIpLmVhY2goZnVuY3Rpb24oKXtcbiAgICAgICAgICAgICAgICBmb3IgKGNvbnN0IGZpbGUgb2YgdGhpcy5maWxlcykge1xuICAgICAgICAgICAgICAgICAgZm9ybURhdGEuYXBwZW5kKGZpbGUubmFtZSwgZmlsZSlcbiAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgIH0pO1xuXG4gICAgICAgICAgICAgICQuYWpheCh7XG4gICAgICAgICAgICAgICAgdHlwZTogXCJQT1NUXCIsXG4gICAgICAgICAgICAgICAgdXJsOiBhY3Rpb24sXG4gICAgICAgICAgICAgICAgZGF0YTogZm9ybURhdGEsXG4gICAgICAgICAgICAgICAgY2FjaGU6IGZhbHNlLFxuICAgICAgICAgICAgICAgIGRhdGFUeXBlOiBcImpzb25cIiwgLy8gZXhwZWN0IGpzb24gZnJvbSB0aGUgc2VydmVyXG4gICAgICAgICAgICAgICAgcHJvY2Vzc0RhdGE6IGZhbHNlLCAvL3RlbGwganF1ZXJ5IG5vdCB0byBwcm9jZXNzIGRhdGFcbiAgICAgICAgICAgICAgICBjb250ZW50VHlwZTogZmFsc2UsIC8vdGVsbCBqcXVlcnkgbm90IHRvIHNldCBjb250ZW50LXR5cGVcbiAgICAgICAgICAgICAgICBzdWNjZXNzOiBmdW5jdGlvbiAoZGF0YSkge1xuICAgICAgICAgICAgICAgICAgdm0uZm9ybSA9IHsgLi4uZGVmYXVsdEZvcm1EYXRhIH07XG4gICAgICAgICAgICAgICAgICB2bS5mb3JtLnJlc3BvbnNlRGF0YSA9IGRhdGE7XG4gICAgICAgICAgICAgICAgICAvLyBpZiB0aGVyZSBhcmUgdmFsaWRhdGlvbiBlcnJvcnMgb24gdGhlIGZvcm0sIGRpc3BsYXkgdGhlbS5cbiAgICAgICAgICAgICAgICAgIGlmIChkYXRhLnZhbGlkYXRpb25FcnJvcikge1xuICAgICAgICAgICAgICAgICAgICAvL2xlZ2FjeVxuICAgICAgICAgICAgICAgICAgICBpZiAoZGF0YS5lcnJvcnNbXCJzZXJ2ZXJWYWxpZGF0aW9uTWVzc2FnZVwiXSAhPSBudWxsKSB7XG4gICAgICAgICAgICAgICAgICAgICAgdm0uZm9ybS5zZXJ2ZXJWYWxpZGF0aW9uTWVzc2FnZSA9XG4gICAgICAgICAgICAgICAgICAgICAgICBkYXRhLmVycm9yc1tcInNlcnZlclZhbGlkYXRpb25NZXNzYWdlXCJdO1xuICAgICAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgICAgIHZtLmZvcm0uc3VibWl0VmFsaWRhdGlvbkVycm9yID0gdHJ1ZTtcbiAgICAgICAgICAgICAgICAgICAgb2JzZXJ2ZXIuc2V0RXJyb3JzKGRhdGEuZXJyb3JzKTtcbiAgICAgICAgICAgICAgICAgICAgcmV0dXJuO1xuICAgICAgICAgICAgICAgICAgfVxuXG4gICAgICAgICAgICAgICAgICAvLyBpZiB0aGUgc2VydmVyIHNlbmRzIGEgcmVkaXJlY3QsIHJlbG9hZCB0aGUgd2luZG93XG4gICAgICAgICAgICAgICAgICBpZiAoZGF0YS5yZWRpcmVjdCkge1xuICAgICAgICAgICAgICAgICAgICB3aW5kb3cubG9jYXRpb24uaHJlZiA9IGRhdGEucmVkaXJlY3Q7XG4gICAgICAgICAgICAgICAgICAgIHJldHVybjtcbiAgICAgICAgICAgICAgICAgIH1cblxuICAgICAgICAgICAgICAgICAgdm0uZm9ybS5zdWJtaXRTdWNjZXNzID0gdHJ1ZTtcbiAgICAgICAgICAgICAgICAgIHZtLmZvcm0uc3VjY2Vzc01lc3NhZ2UgPSBkYXRhLnN1Y2Nlc3NNZXNzYWdlO1xuICAgICAgICAgICAgICAgICAgcmV0dXJuO1xuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgZXJyb3I6IGZ1bmN0aW9uICh4aHIsIHN0YXR1c1RleHQpIHtcbiAgICAgICAgICAgICAgICAgIHZtLmZvcm0gPSB7IC4uLmRlZmF1bHRGb3JtRGF0YSB9O1xuICAgICAgICAgICAgICAgICAgdm0uZm9ybS5zdWJtaXRFcnJvciA9IHRydWU7XG4gICAgICAgICAgICAgICAgICB2bS5mb3JtLnNlcnZlckVycm9yTWVzc2FnZSA9IGAke3hoci5zdGF0dXN9ICR7c3RhdHVzVGV4dH1gO1xuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgfVxuICAgICAgICAgIH0pO1xuICAgICAgICAgIHJldHVybiBmYWxzZTtcbiAgICAgICAgfSxcbiAgICAgIH0sXG4gICAgfSk7XG4gIH0pO1xuXG4gIC8vIHJ1biB0aGUgdnVlLWZvcm0gaW5pdCBzY3JpcHQgcHJvdmlkZWQgaW4gdGhlIE9DIGFkbWluIHVpXG4gIGxldCBpbml0U2NyaXB0ID0gYXBwLmRhdGFzZXQuaW5pdFNjcmlwdDtcbiAgaWYgKGluaXRTY3JpcHQpIHtcbiAgICBjb25zdCBpbml0Rm4gPSBuZXcgRnVuY3Rpb24oZGVjb2RlVW5pY29kZShpbml0U2NyaXB0KSk7XG4gICAgaW5pdEZuKCk7XG4gIH1cbn1cblxuLy8gbG9vayBmb3IgYWxsIHZ1ZSBmb3JtcyB3aGVuIHRoaXMgc2NyaXB0IGlzIGxvYWRlZCBhbmQgaW5pdGlhbGl6ZSB0aGVtXG5kb2N1bWVudC5xdWVyeVNlbGVjdG9yQWxsKFwiLnZ1ZS1mb3JtXCIpLmZvckVhY2goaW5pdEZvcm0pO1xuXG5kb2N1bWVudC5hZGRFdmVudExpc3RlbmVyKFwiRE9NQ29udGVudExvYWRlZFwiLCBmdW5jdGlvbiAoZXZlbnQpIHtcblxuICBkb2N1bWVudC5xdWVyeVNlbGVjdG9yQWxsKFwiLnZ1ZS1hcHAtaW5zdGFuY2VcIikuZm9yRWFjaChmdW5jdGlvbiAoZWxlbSkge1xuICAgIG5ldyBWdWUoe1xuICAgICAgZWw6IGVsZW0sXG4gICAgfSk7XG4gIH0pO1xuXG4gIGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3JBbGwoXCIudnVldGlmeS1hcHAtaW5zdGFuY2VcIikuZm9yRWFjaChmdW5jdGlvbiAoZWxlbSkge1xuICAgIG5ldyBWdWUoe1xuICAgICAgZWw6IGVsZW0sXG4gICAgICB2dWV0aWZ5OiBuZXcgVnVldGlmeSgpLFxuICAgIH0pO1xuICB9KTtcblxufSk7XG4iXX0=
