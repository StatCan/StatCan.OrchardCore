"use strict";

function decodeUnicode(str) {
  return decodeURIComponent(atob(str).split('').map(function (c) {
    return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));
}


// run init script
function initForm(app) {

  // Set VeeValidate language based on the lang parameter
  VeeValidate.localize(app.dataset.lang);

  let componentOptions = app.dataset.options;

  let parsedOptions = {};
  if(componentOptions)  {
    const fn = new Function(`return ${decodeUnicode(componentOptions)};`);
    parsedOptions = fn();
  }

  const { data: parsedData, methods: parsedMethods, ...parsedRest } = parsedOptions;
  let objData = parsedData;
  if(typeof parsedData === "function")
  {
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
    responseData: undefined
  }

  Vue.component(app.dataset.name, {
    // First because the elements below will override
    ...parsedRest,
    template: `#${app.dataset.name}`,
    data: function () { 
      return {
        ...objData,
        form: {...defaultFormData}
      };
    },
    methods: {
      // default method that return the data to be submitted to the server
      // this was added first to allow the Administrator to edit this function on the OC Admin
      submitData() {
        return {...this.$data};
      },
      ...parsedMethods,
      formReset() {
        this.form = {...defaultFormData};
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
            let frmData = submitData();
            frmData.__RequestVerificationToken = vm.$refs.form.querySelector('input[name="__RequestVerificationToken"]').value;
            if(typeof(grecaptcha) == 'object')
            {
              frmData.recaptcha = grecaptcha.getResponse();
            }

            vm.form.submitting = true;
            
            $.ajax({
              type: "POST",
              url: action,
              data: frmData,
              cache: false,
              dataType: "json",
              success: function (data) {

                vm.form = {...defaultFormData};
                vm.form.responseData = data;
                // if there are validation errors on the form, display them.
                if (data.validationError) {
                  
                  //legacy
                  if(data.errors['serverValidationMessage'] != null) {
                    vm.form.serverValidationMessage = data.errors['serverValidationMessage'];
                  }
                  vm.form.submitValidationError = true;
                  observer.setErrors(data.errors);
                  return;
                }

                // if the server sends a redirect, reload the window
                if (data.redirect) {
                  window.location.href = data.redirect;
                  return;
                }

                vm.form.submitSuccess = true;
                vm.form.successMessage = data.successMessage;
                return;
                
              },
              error: function (xhr, statusText) {
                vm.form = {...defaultFormData};
                vm.form.submitError = true;
                vm.form.serverErrorMessage = `${xhr.status} ${statusText}`;
              }
            });
          }
        });
        return false;
      }
    }
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

document.addEventListener("DOMContentLoaded", function(event) { 
  const formLoadedEvent = new Event('vue-form-component-loaded');
  document.dispatchEvent(formLoadedEvent);
});
 
