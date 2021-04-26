$(".link-feedback, .close-feedback").click(function () {
    if ($("#feedBack").hasClass("feedbackCenter")) {
        $("#feedBack").removeClass("feedbackCenter");
        $("#feedBack").hide();
        $(".link-feedback").css("margin-right", "0");
        $(".link-feedback").css("right", "");
    } else {
        $("#feedBack").addClass("feedbackCenter");
        $("#feedBack").show();
        $(".link-feedback").css("right", "50%");
        $(".link-feedback").css("margin-right", "190px");
        initValidation($("form"));
    }
});


