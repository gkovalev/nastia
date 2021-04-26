var mpBuyInOneClick;

$(function () {
    if ($("#modalBuyInOneClick").length) {
        mpBuyInOneClick = $.advModal({
            title: localize("buyInOneClick_Confirm"),
            htmlContent: $("#modalBuyInOneClick"),
            control: "#lBtnBuyInOneClick",
            afterOpen: function () { initValidation($("form")); },
            afterClose: function () { confirmationProductInOneClickFinal(); initValidation($("form")); },
beforeClose: function () { confirmationProductInOneClickFinal(); },
clickOut: false
        });
    }

    if ($("#btnBuyInOneClick").length) {
        $("#btnBuyInOneClick").click(function () {
            if ($('form').valid('buyInOneClick') === true) {
                confirmationProductInOneClick(
                    $("#hfProductId").val(),
                    $("#customOptionsHidden_" + $("#hfProductId").val()).length > 0 ? $("#customOptionsHidden_" + $("#hfProductId").val()).val() : null,
                    $("#txtAmount").val(),
                    $("#txtName").val(),
                    $("#txtAdres").val(),
                    $("#txtMail").val(),
                    $("#txtPhone").val(),
                    $("#txtComment").val(),
                    $("#txtCity").val(),
                    $("#txtfamilyName").val(),
                    $("#txtPayment").val(),
                    $("#txtShipping").val(),
                    $("#lBtnBuyInOneClick").attr("data-page"));
            }

        });
    }
    if ($("#btnBuyInOneClickOk").length) {
        $("#btnBuyInOneClickOk").click(function () {
mpBuyInOneClick.modalClose();
            confirmationProductInOneClickFinal();
        });
    }
});


function confirmationProductInOneClick(productId, customOptions, amount, name, adres, email, phone, comment, city, familyName, payment, shipping, page) {
    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
async: false,
        data: {
            productId: htmlEncode(productId),
            customOptions: htmlEncode(customOptions),
            amount: htmlEncode(amount),
            name: htmlEncode(name),
            adres: htmlEncode(adres),
            email: htmlEncode(email),
            phone: htmlEncode(phone),
            comment: htmlEncode(comment),
            city: htmlEncode(city),
            familyName: htmlEncode(familyName),
            payment: htmlEncode(payment),
            shipping: htmlEncode(shipping),
            page: htmlEncode(page)
        },
        url: "httphandlers/buyinoneclick.ashx",
        beforesend: function () {
            $("#modalBuyInOneClickFinal").hide();
            $("#modalBuyInOneClickForm").show();
        },
        success: function (data) {
            $("#modalBuyInOneClickFinal").show();
            $("#modalBuyInOneClickForm").hide();
            if (data!= null && data.result == "reload") {
                if ($("#btnBuyInOneClickOk").length) {
                    $("#btnBuyInOneClickOk").click(function () {
                        confirmationProductInOneClickFinalReload();
                    });
                }
            }
        },
        error: function () {
            mpBuyInOneClick.modalClose();
        }
    });
}

function confirmationProductInOneClickFinal() {
    
    $("#modalBuyInOneClickFinal").hide();
    $("#modalBuyInOneClickForm").show();
    $("#txtName").val("");
    $("#txtAdres").val("");
    $("#txtEmail").val("");
    $("#txtComment").val("");
}
function confirmationProductInOneClickFinalReload() {
    mpBuyInOneClick.modalClose();
    window.location = $("base").attr("href");

}