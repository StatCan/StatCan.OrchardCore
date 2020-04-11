(function($) {
  "use strict";
  //IE Polyfill for forEach on NodeList
  if (window.NodeList && !NodeList.prototype.forEach) {
    NodeList.prototype.forEach = function(callback, thisArg) {
      thisArg = thisArg || window;
      for (let i = 0; i < this.length; i++) {
        callback.call(thisArg, this[i], i, this);
      }
    };
  }
  // IE Polyfill for closest()
  if (!Element.prototype.matches) {
    Element.prototype.matches =
      Element.prototype.msMatchesSelector ||
      Element.prototype.webkitMatchesSelector;
  }

  if (!Element.prototype.closest) {
    Element.prototype.closest = function(s) {
      let el = this;

      do {
        if (el.matches(s)) return el;
        el = el.parentElement || el.parentNode;
      } while (el !== null && el.nodeType === 1);
      return null;
    };
  }
  // debounce fn
  function debounce(func, wait, immediate) {
    let timeout;
    return function() {
      let context = this,
        args = arguments;
      let later = function() {
        timeout = null;
        if (!immediate) func.apply(context, args);
      };
      let callNow = immediate && !timeout;
      clearTimeout(timeout);
      timeout = setTimeout(later, wait);
      if (callNow) func.apply(context, args);
    };
  }

  $.fn.serializeObject = function() {
    let o = {};
    let a = this.serializeArray();
    $.each(a, function() {
      if (o[this.name] !== undefined) {
        if (!o[this.name].push) {
          o[this.name] = [o[this.name]];
        }
        o[this.name].push(this.value || "");
      } else {
        o[this.name] = this.value || "";
      }
    });
    return o;
  };

  $(document).ready(function() {
    const lang = document.documentElement.lang;

    /**
     * Initialize forms
     */
    document.querySelectorAll(".ajax-form").forEach(function(form) {

      const $form = $(form);
      let serverValidationDiv = form.querySelector(".form-server-messages");
      if(!serverValidationDiv){
        // if the user did not specify a serverValidationMessages div inject a default one.
        serverValidationDiv = $form.append('<div></div>');
      }
      const formElements = form.elements;

      form.addEventListener(
        "submit",
        function(event) {
          event.preventDefault();
          event.stopPropagation();
          // validate the form
          if (form.checkValidity()) {
            // disable elements that have the data-disable-submit attribute
            const toDisable = form.querySelectorAll("[data-disable-submit='true']");
            toDisable.forEach(x =>x.setAttribute('disabled', true));

            const serializedForm = $form.serialize();

            // if the user enabled debug values, display form values being submitted to the server
            if (form.dataset.debugValues) {
              let debugValuesDiv = form.querySelector(".debug-values");
              if(!debugValuesDiv){
                // if the user did not specify a serverValidationMessages div inject a default one.
                debugValuesDiv = $form.append('<div class="debug-values"></div>');
              }
              const warningDiv = $(
                '<div class="alert alert-warning" role="alert"> \
                ' +
                  "<p>Debug values: </p><div><pre>" +
                  serializedForm.replace(/[&]/g, "\n") +
                  "</pre></div>" +
                  " \
                </div>"
              );
              debugValuesDiv.innerHTML = warningDiv.prop("outerHTML");
            }

            // Post to the form controller
            $.ajax({
              type: "POST",
              url: form.action,
              data: serializedForm,
              cache: false,
              dataType: "json",
              success: function(data) {
                let successMsg;
                if (data) {
                  if (data.redirect) {
                    setTimeout(()=>{window.location.href = data.redirect},2000);
                  }
                  if (data.success) {
                    if (typeof data.success === "object") {
                      // object with keys being the language
                      successMsg = data.success[lang];
                    } else if (typeof data.success === "string") {
                      successMsg = data.success;
                    }
                  }
                }
                const div = $(
                  '<div class="alert alert-success" role="alert">' +
                    (successMsg ||
                      "Please specify a default success message!!") +
                    "</div>"
                );
                serverValidationDiv.innerHTML = div.prop("outerHTML");
              },
              error: function(xhr) {
                let err;
                const data = xhr.responseJSON;
                if (data) {
                  if (data.redirect) {
                    setTimeout(()=>{window.location.href = data.redirect},2000);
                  }
                  if (data.error) {
                    if (typeof data.error === "object") {
                      // object with keys being the language
                      err = data.error[lang];
                    } else if (typeof data.error === "string") {
                      err = data.error;
                    }
                  }
                }
               
                const errDiv = $(
                  '<div class="alert alert-danger" role="alert">' +
                    (err ||
                      "Please specify a default error message!!") +
                    "</div>"
                );
                toDisable.forEach(x=>x.removeAttribute("disabled"));
                serverValidationDiv.innerHTML = errDiv.prop("outerHTML");
              }
            });
          } else {
            // validation on submit failed. Set the class of the wrapper
            for (let i = 0; i < formElements.length; i += 1) {
              const elem = formElements[i];
              const wrapper = elem.closest(".form-control-wrapper");
              if (wrapper) {
                if (!elem.validity.valid) {
                  wrapper.classList.remove("valid");
                  wrapper.classList.add("invalid");
                } else {
                  wrapper.classList.remove("invalid");
                  wrapper.classList.add("valid");
                }
              }
            }
            // focus on first invalid input
            $form
              .find(":invalid")
              .first()
              .focus();
          }
          // this is added on first submit so that the form errors are
          form.classList.add("was-validated");
        },
        false
      );
    });
  });
})(jQuery);
