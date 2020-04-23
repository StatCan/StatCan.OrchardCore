(function($) {
  "use strict";

  // set valid / error classes for the input to be bs4 classes
  var settings = {
    validClass: "is-valid",
    errorClass: "is-invalid"

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
    // parse the form unobtrusive 
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

              console.log("Success", data.html);

              $form.replaceWith(data.html);
              // need to reparse the form
              initForm(document.getElementById(formId));

              // todo: handle redirect ?


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
