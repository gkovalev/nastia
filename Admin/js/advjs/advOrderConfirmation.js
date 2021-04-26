var mpAddressOC;

var advantshop = Advantshop;
var scriptManager = advantshop.ScriptsManager;

$(function () {
    mpAddressOC = $.advModal({
        control: "a.btn-add-adr-my",
        htmlContent: $("#modal"),
        title: localize("orderConfirnationAddress"),
        beforeClose: function () {
            $("form").data("validator").hideErrors();
        },
        afterOpen: function () { initValidation($("form"), "address"); }
    });

    $("#btnAddChangeContactOc").click(function () {
        if ($("form").valid("address")) {
            addUpdateContactForOc();
        } else {
            return false;
        }
    });

    $("#btnAddAddress").click(function () {
        ShowModalAddAddressOCAdd();
    });

    if ($('#chkBillingIsShipping').is(":checked")) {
        $('#pnBilling').hide();
    } else {
        $('#pnBilling').show();
    }

    initValidation($("form"));

});

function showHideBillingPanel(panel) {
    $(panel).toggle();
    initValidation($("form"));
}

function ShowModalAddAddressOCAdd() {
    $("#txtContactNameOc").val("");
    $("#txtContactZoneOc").val("");
    $("#txtContactCityOc").val("");
    $("#txtContactAddressOc").val("");
    $("#txtContactZipOc").val("");
    $("#txtContactPhoneOc").val("");
    $("#hfContactIdOc").val("");
    $("#btnAddChangeContactOc").text(localize("orderConfirnationAdd"));

}

function ShowModalAddAddressOC(fio, country, region, city, address, postcode, contactid, event) {
    $("#txtContactNameOc").val(fio);
    $("#cboCountryOc").val(country);
    $("#txtContactZoneOc").val(region);
    $("#txtContactCityOc").val(city);
    $("#txtContactAddressOc").val(address);
    $("#txtContactZipOc").val(postcode);
    $("#hfContactIdOc").val(contactid);
    $("#btnAddChangeContactOc").text(localize("orderConfirnationChange"));

    event = event || window.event;
    event.stopPropagation ? event.stopPropagation() : event.cancelBubble = true;
    mpAddressOC.modalShow();
}

function showHideAdressPayment() {
    var div = $(".adress-payment");
    if (div.is(":hidden")) {
        div.show();
        $("#hfBillingIsShippingOc").val("0");
        if ($('input[name=adrP]').length)
            $('input[name=adrP]').click();
    } else {
        $("#hfBillingIsShippingOc").val("1");
        div.hide();
        $("#hfContactBillingId").val("");
    }
}

function setContactShippingId(id, radioId) {
    $("#hfOcContactShippingId").val(id);
    $("#" + radioId).attr("checked", "checked");
}

function setContactBillingId(id, radioId) {
    $("#hfOcContactBillingId").val(id);
    $("#" + radioId).attr("checked", "checked");

}

function getContactsForOC(containerId, selectedShippingContact, selectedBillingContact, billingIsShipping) {
    var itemHtml = "<li onclick=\"setContactShippingId('{7}','a{8}');\"> \
                        <input type=\"radio\" name=\"adr\" id=\"a{8}\" {10}/> \
                        <span for=\"a{8}\">{0}, {4}, {5}</span> \
                            <a class=\"link-edit-a address-edit-payment\" href=\"javascript:void(0)\" onclick=\"ShowModalAddAddressOC('{1}','{2}','{3}','{4}','{5}','{6}','{7}', event);return false;\">{11}</a> \
                            <a href=\"#\" class=\"link-remove-a\" onclick=\"delContactForOc('{7}','{9}'); return false;\">{12}</a> \
                    </li>";
    var itemPayerHtml = "<li onclick=\"setContactBillingId('{7}', 'ap{8}')\"> \
                            <input type=\"radio\" name=\"adrP\" id=\"ap{8}\" {10}/> \
                            <span for=\"ap{8}\">{0}, {4}, {5}</span> \
                            <a class=\"link-edit-a address-edit-billing\" href=\"javascript:void(0)\" onclick=\"ShowModalAddAddressOC('{1}','{2}','{3}','{4}','{5}','{6}','{7}', event);return false;\">{11}</a> \
                            <a href=\"#\" class=\"link-remove-a\" onclick=\"delContactForOc('{7}','{9}'); return false;\">{12}</a> \
                         </li>";
    var conactsHtml = "";
    var contactsPayerHtml = "";

    var progressMini = new scriptManager.Progress.prototype.Init($(containerId));

    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
        url: "httphandlers/myaccount/getcustomercontacts.ashx",
        success: function (data) {
            if (data.length > 0) {
                if (billingIsShipping) {
                    $("#hfBillingIsShippingOc").val(billingIsShipping);
                } else {
                    billingIsShipping = $("#hfBillingIsShippingOc").val();
                }
                var isFirst = true;
                $.each(data, function (idx, c) {
                    if (c.CustomerContactID == selectedShippingContact || isFirst) {
                        conactsHtml += String.Format(itemHtml, c.Country, c.Name, c.CountryId, c.RegionName, c.City, c.Address, c.Zip,  c.CustomerContactID, idx, containerId, "checked=\"checked\"", localize("orderConfirnationEdit"), localize("orderConfirnationDelete"));
                        isFirst = false;
                        setContactShippingId(c.CustomerContactID, idx);
                    }
                    else {
                        conactsHtml += String.Format(itemHtml, c.Country, c.Name, c.CountryId, c.RegionName, c.City, c.Address, c.Zip,  c.CustomerContactID, idx, containerId, "", localize("orderConfirnationEdit"), localize("orderConfirnationDelete"));
                    }
                });
                isFirst = true;
                $.each(data, function (idx, c) {
                    if (c.CustomerContactID == selectedBillingContact || isFirst) {
                        contactsPayerHtml += String.Format(itemPayerHtml, c.Country, c.Name, c.CountryId, c.RegionName, c.City, c.Address, c.Zip, c.CustomerContactID, idx, containerId, "checked=\"checked\"", localize("orderConfirnationEdit"), localize("orderConfirnationDelete"));
                        isFirst = false;
                        setContactBillingId(c.CustomerContactID, idx);
                    } else {
                        contactsPayerHtml += String.Format(itemPayerHtml, c.Country, c.Name, c.CountryId, c.RegionName, c.City, c.Address, c.Zip,  c.CustomerContactID, idx, containerId, "", localize("orderConfirnationEdit"), localize("orderConfirnationDelete"));
                    }
                });
                $(containerId).html(String.Format("<div class=\"title\">{2}</div> \
                                 <ul class=\"list-adress\">{0} \
                                    <li> \
                                        <input type=\"checkbox\" id=\"adr-too\" class=\"checkbox-adress-payment\" {5} onClick=\"showHideAdressPayment()\" /> \
                                        <label for=\"adr-too\">{3}</label> \
                                        <div class=\"adress-payment\" style=\"{6}\"> \
                                            <div class=\"title\">{4}</div> \
                                            <ul class=\"list-adress\">{1}</ul> \
                                        </div> \
                                     </li> \
                                  </ul>"
                                  , conactsHtml, contactsPayerHtml, localize("orderConfirnationShippingAddress"), localize("orderConfirnationShippingAddressMatch"), localize("orderConfirnationBillingAddress"),
                                    billingIsShipping == "1" ? "checked=\"checked\"" : "",
                                    billingIsShipping == "1" ? "display:none;" : ""));
            }
            else {
                window.location.reload();
            }
        },
        beforeSend: function () {
            progressMini.Show();
            },
        complete: function () {
            progressMini.Hide();
        },
        error: function () {
        }
    });
}

function delContactForOc(id, containerId, callback) {
    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
        url: "httphandlers/myaccount/deletecustomercontact.ashx?contactid=" + id,
        success: function () {
            if (callback != null) {
                callback(containerId);
            } else {
                getContactsForOC(containerId);
            }
        },
        beforeSend: function () {
        },
        complete: function () {
        },
        error: function () {
        }
    });
}

function addUpdateContactForOc(callback) {
    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
        data: {
            fio: htmlEncode($("#txtContactNameOc").val()),
            countryid: htmlEncode($("#cboCountryOc :selected").val()),
            country: htmlEncode($("#cboCountryOc :selected").text()),
            region: htmlEncode($("#txtContactZoneOc").val()),
            city: htmlEncode($("#txtContactCityOc").val()),
            address: htmlEncode($("#txtContactAddressOc").val()),
            zip: htmlEncode($("#txtContactZipOc").val()),
            contactid: htmlEncode($("#hfContactIdOc").val())
        },
        url: "httphandlers/myaccount/addupdatecustomercontact.ashx",
        success: function () {
            mpAddressOC.modalClose();
            if (callback != null) {
                callback("#contactsDivOc");
            } else {
                getContactsForOC("#contactsDivOc");
            }
        },
        error: function (data) {
            notify(localize("orderConfirnationShippingErrorUpdate") + " status text:" + data.statusText, notifyType.error, true);
            mpAddressOC.modalClose();
        }
    });
}

function setSelectedCountry(ddlId, ddlBillingId, billingIsShipping) {
    $("#hfSelectedCountry").val($("#" + ddlId + " option:selected").val() + ";" + $("#" + ddlId + " option:selected").text());
    if ($("#" + billingIsShipping).attr("checked") != "checked") {
        $("#hfSelectedCountryBilling").val($("#" + ddlId + " option:selected").val() + ";" + $("#" + ddlId + " option:selected").text());
    }
}

function getPaymentButton(orderId, containerId) {
    $.ajax({
        dataType: "json",
        cache: false,
        type: "POST",
        data: {
            orderid: htmlEncode(orderId)
        },
        url: "httphandlers/orderconfirmation/getpaymentbutton.ashx",
        success: function (data) {
            if (data != null) {
                $("#" + containerId).html(data.buttonString);
                $('form').after(data.formString);
            }

            if (window.PIELoad)
                window.PIELoad();
        },
        error: function () {

        }
    });
}

function formGenerate(form) {
    var container = $("#btnPaymentFunctionality");

    if (!container.length) return;

    container.html(form);

}

function UpdateCustomerEmail() {
    if ($("form").valid("email")) {
        var res = false;
        $.ajax({
            type: "POST",
            url: "httphandlers/myaccount/updatecustomeremail.ashx",
            data: { email: $("#customerEmail").val() },
            dataType: "json",
            async: false,
            cache: false,
            error: function (data) {
                throw new Error(data);
            },
            success: function (data) {
                res = data;
            }
        });
    }
    return res;
}




