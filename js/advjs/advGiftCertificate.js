$(function () {
    if ($("#modalGiftCertificate").length) {
        $.advModal({
            htmlContent: $("#modalGiftCertificate"),
            control: "#printCert",
            beforeOpen: ShowModalPreviewGiftCertificate,
            afterOpen: function () {
                PIELoad(".certificate, .certificate .section");
            },
            title: localize("giftcertificateTitle")
        });
    }
});

function ShowModalPreviewGiftCertificate() {
    getStingPrice($("#txtSum").val(), function (price) {
                                            $("#lblToName").text($("#txtTo").val());
                                            $("#lblFromName").text($("#txtFrom").val());
                                            $("#lblMessage").html($("#txtMessage").val().replace(/\n/g, "<br />"));
                                            $("#lblSum").html(price);
                                        }
    );
    
}
