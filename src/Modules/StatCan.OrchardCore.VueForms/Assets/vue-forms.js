"use strict";

function decodeUnicode(str) {
  return decodeURIComponent(
    atob(str)
      .split("")
      .map(function (c) {
        return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
      })
      .join("")
  );
}

// run init script
function initForm(app) {
  // Set VeeValidate language based on the lang parameter
  VeeValidate.localize(app.dataset.lang);

  let componentOptions = app.dataset.options;

  let parsedOptions = {};
  if (componentOptions) {
    const fn = new Function(`return (${decodeUnicode(componentOptions)});`);
    parsedOptions = fn();
    if (!parsedOptions) {
      console.log(
        "Could not parse the componentOptions object. Make sure your object is well formed."
      );
    }
  }

  const {
    data: parsedData,
    methods: parsedMethods,
    ...parsedRest
  } = parsedOptions;
  let objData = parsedData;
  if (typeof parsedData === "function") {
    objData = parsedData();
  }
  const defaultFormData = {
    submitting: false,
    submitSuccess: false,
    successMessage: undefined,
    submitError: false,
    submitValidationErrors: false,
    serverValidationMessage: undefined,
    serverErrorMessage: undefined,
    responseData: undefined,
  };

  const componentTemplate = decodeUnicode(app.dataset.template);

  Vue.component(app.dataset.name, function (resolve) {
    resolve({
      // First because the elements below will override
      ...parsedRest,
      template: `${componentTemplate}`,
      data: function () {
        return {
          ...objData,
          form: { ...defaultFormData }
        };
      },
      methods: {
        // default method that return the data to be submitted to the server
        // this was added first to allow the Administrator to edit this function on the OC Admin
        submitData() {
          let clonedData = { ...this.$data };
          delete clonedData.form;
          return clonedData;
        },
        ...parsedMethods,
        formReset() {
          this.form = { ...defaultFormData };
          // also reset the VeeValidate observer
          this.$refs.obs.reset();
        },
        formHandleSubmit(e) {
          e.preventDefault();
          const vm = this;
          // keep a reference to the VeeValidate observer
          const observer = vm.$refs.obs;
          observer.validate().then((valid) => {
            if (valid) {
              const action = vm.$refs.form.getAttribute("action");
               
              // set form vue data
              vm.form.submitting = true;

              let frmData = vm.submitData();
              frmData.__RequestVerificationToken = vm.$refs.form.querySelector(
                'input[name="__RequestVerificationToken"]'
              ).value;
              if (typeof grecaptcha == "object") {
                frmData.recaptcha = grecaptcha.getResponse();
              }

              let formData = window.serializeToFormData(frmData);

              // iterate all file inputs and add the files to the request
              $(this.$refs.form).find("input[type=file]").each(function(){
                for (const file of this.files) {
                  formData.append(file.name, file)
                }
              });

              $.ajax({
                type: "POST",
                url: action,
                data: formData,
                cache: false,
                dataType: "json", // expect json from the server
                processData: false, //tell jquery not to process data
                contentType: false, //tell jquery not to set content-type
                success: function (responseData) {
                  vm.form = { ...defaultFormData };
                  vm.form.responseData = responseData;

                  if(responseData.debug)
                  {
                    console.log("Debug object: ", responseData.debug);
                  }

                  // if there are validation errors on the form, display them.
                  if (responseData.validationError) {
                    //legacy
                    if (responseData.errors["serverValidationMessage"] != null) {
                      vm.form.serverValidationMessage =
                        responseData.errors["serverValidationMessage"];
                    }
                    vm.form.submitValidationError = true;
                    observer.setErrors(responseData.errors);
                    return;
                  }

                  // if the server sends a redirect, reload the window
                  if (responseData.redirect) {
                    window.location.href = responseData.redirect;
                    return;
                  }

                  vm.form.submitSuccess = true;
                  vm.form.successMessage = responseData.successMessage;
                  return;
                },
                error: function (xhr, statusText) {
                  vm.form = { ...defaultFormData };
                  vm.form.submitError = true;
                  vm.form.serverErrorMessage = `${xhr.status} ${statusText}`;
                  console.log("An error occurred while executing the request", xhr.responseText);
                },
              });
            }
          });
          return false;
        },
      }
    });
  });

  // run the vue-form init script provided in the OC admin ui
  let initScript = app.dataset.initScript;
  if (initScript) {
    const initFn = new Function(decodeUnicode(initScript));
    initFn();
  }
}

// look for all vue forms when this script is loaded and initialize them
document.querySelectorAll(".vue-form").forEach(initForm);

document.addEventListener("DOMContentLoaded", function (event) {

  document.querySelectorAll(".vue-app-instance").forEach(function (elem) {
    new Vue({
      el: elem,
    });
  });

  document.querySelectorAll(".vuetify-app-instance").forEach(function (elem) {
    new Vue({
      el: elem,
      vuetify: new Vuetify(),
    });
  });

});
