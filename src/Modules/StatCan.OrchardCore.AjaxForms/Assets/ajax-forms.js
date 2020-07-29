(function($) {
  "use strict";

  // set valid / error classes for the input to be bs4 classes
  var settings = {
    //validClass: "is-valid", //This is disabled, we do not want to highlight valid inputs.
    errorClass: "is-invalid",
    onkeyup: false,
    onfocusout: false,
    onclick: false
  };
  $.validator.setDefaults(settings);
  $.validator.unobtrusive.options = settings;

  //IE Polyfill for forEach on NodeList
  if (window.NodeList && !NodeList.prototype.forEach) {
    NodeList.prototype.forEach = function(callback, thisArg) {
      thisArg = thisArg || window;
      for (let i = 0; i < this.length; i++) {
        callback.call(thisArg, this[i], i, this);
      }
    };
  }

  function initForm(form) {

    let $form = $(form);
    // parse the form with unobtrusive library 
    $.validator.unobtrusive.parse($form);
    const formId = $form.attr("id");

    $form.submit(function (event) {        
        if ($form.valid()) {
          // form has been validated by jQuery validation
          event.preventDefault();
          event.stopPropagation();
          // disable elements that have the data-disable-submit attribute
          const toDisable = form.querySelectorAll("[data-disable-submit='true']");
          toDisable.forEach(x => x.setAttribute('disabled', true));

          const serializedForm = $form.serialize();

          // Post to the form controller
          $.ajax({
            type: "POST",
            url: form.action,
            data: serializedForm,
            cache: false,
            dataType: "json",
            success: function (data) {


              if (data.validationError) {
                // Server side validation can occur, form is re-rendered
                $form.replaceWith(data.html);
                // need to reparse the form
                initForm(document.getElementById(formId));
                //TODO: focus on first error or validation summary
              }
              // if the server sends a redirect, reload the window
              if (data.redirect) {
                window.location.href = data.redirect;
              }

              //TODO: Handle default success case, maybe show some

            },
            error: function (xhr) {
              // todo: handle this case gracefully, usually in the case of server error.
              const data = xhr.responseJSON;
              console.log("error", data);
            }
          });
        }
      }
    );
  }

  $(document).ready(function () {
    // look for all ajax forms on load and initialize them
    document.querySelectorAll(".ajax-form").forEach(initForm);
  });
})(jQuery);
