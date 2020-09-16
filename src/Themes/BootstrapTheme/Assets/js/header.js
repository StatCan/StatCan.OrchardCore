(function($) {
  "use strict"; // Start of use strict

  // Smooth scrolling using jQuery easing
  $('a.js-scroll-trigger[href*="#"]:not([href="#"])').click(function() {
    if (
      location.pathname.replace(/^\//, "") ==
        this.pathname.replace(/^\//, "") &&
      location.hostname == this.hostname
    ) {
      var target = $(this.hash);
      target = target.length ? target : $("[name=" + this.hash.slice(1) + "]");
      if (target.length) {
        $("html, body").animate(
          {
            scrollTop: target.offset().top - 54
          },
          1000,
          "easeInOutExpo"
        );
        return false;
      }
    }
  });

  function getDocHeight() {
    var D = document;
    return Math.max(
        D.body.scrollHeight, D.documentElement.scrollHeight,
        D.body.offsetHeight, D.documentElement.offsetHeight,
        D.body.clientHeight, D.documentElement.clientHeight
    );
}

function animate(e) {
  var scroll = $(window).scrollTop();
  if (scroll + $(window).height() >= getDocHeight() - 75) {
    $('#mainFooter').removeClass("hide");
    $('#expandedFooter').removeClass("d-none").addClass("d-flex");
  } else if (scroll >= 5) {
    $('#mainHeader').addClass("hide");
    $('#mainFooter').addClass("hide");
    $('#expandedFooter').removeClass("d-none").addClass("d-flex");
  } else {
    $('#mainHeader').removeClass("hide");
    $('#mainFooter').removeClass("hide");
    $('#expandedFooter').addClass("d-none").removeClass("d-flex")
  }
}

$(window).on("load scroll resize", animate);

})(jQuery); // End of use strict
