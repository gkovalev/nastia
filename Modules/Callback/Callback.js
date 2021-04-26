(function($) {
    
    var modalWindow;
    
    $(function () {
        var callbackOptions = $("[data-callback-options]");

        if (callbackOptions.length > 0) {

            var content = new EJS({ url: 'modules/Callback/form.html' }).render({text:callbackOptions.attr("data-callback-text")});

            modalWindow = $.advModal({
                title: callbackOptions.attr("data-callback-title"),
                htmlContent: content,
                control: "[data-callback=true]",
                afterOpen: function() { initValidation($("form")); }
            });
        }
    
        $('#aCallbackSend').on('click', function() {
        if ($('form').valid('callback') === true) {
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                async: false,
                data: {
                    name: htmlEncode($("#txtCallbackName").val()),
                    phone: htmlEncode($("#txtCallbackPhone").val()),
                    comment: htmlEncode($("#txtCallbackComment").val()),
                },
                url: "modules/callback/callbackhandler.ashx",
                success: function(data) {
                    if (data == true) {
                        $("#txtCallbackName").val("");
                        $("#txtCallbackName").val("");
                        $("#txtCallbackName").val("");
                    
                        $(".callback .callback-form").hide();
                        $(".callback .result").show();
                    }
                }
            });
        }
    });

    $("#aCallbackOk").on('click', function() {
        modalWindow.modalClose();
    });
});


})(jQuery);

