(function ($) {
    jQuery.fn.advShoppingCart = function (o) {
        var progressObj = Advantshop.ScriptsManager.Progress;

        $("a.cross.deleteitem", "#shoppingCartTable").live("click", function () {
            var id = $(this).metadata({ type: 'attr', name: 'rel' }).itemId;
            $.ajax({
                dataType: "text",
                cache: false,
                data: { itemid: id },
                type: "POST",
                url: "httphandlers/shoppingcart/deletefromcart.ashx",
                success: function (data) {
                    getCartFull();
                    $.advMiniCartRefresh();
                },
                error: function (data) {
                    alert("error delete");
                }
            });
        });

        $(".link-recal").click(function () {
            var list = "";
            $("tr[id^=itemid_]", "#shoppingCartTable").each(function () {
                var row = $(this);
                var itemId = parseInt(row.attr("id").replace("itemid_", ""));
                var amount = parseInt(row.find("span.spinbox input").val());
                list += itemId + "_" + amount + ";";
            });

            $.ajax({
                dataType: "text",
                cache: false,
                data: { list: list },
                type: "POST",
                url: "httphandlers/shoppingcart/updatecart.ashx",
                success: function (data) {
                    getCartFull();
                    $.advMiniCartRefresh();
                },
                error: function (data) {
                    alert("error recalc");
                }
            });
        });

        $("#delete-all").click(function () {
            $.ajax({
                dataType: "text",
                cache: false,
                type: "POST",
                url: "httphandlers/shoppingcart/clearcart.ashx",
                success: function (data) {
                    getCartFull();
                    $.advMiniCartRefresh();
                },
                error: function (data) {
                    notify("error clearcart" + " status text:" + data.statusText, notifyType.error, true);
                }
            });
        });

        $("a.deletecoupon").live("click", function () {
            $.ajax({
                dataType: "text",
                cache: false,
                type: "POST",
                url: "httphandlers/shoppingcart/deletecoupon.ashx",
                success: function (data) {
                    //getCartFull();
                    //$.advMiniCartRefresh();
                    window.location.reload(true);
                },
                error: function (data) {
                    notify("error deletecoupon" + " status text:" + data.statusText, notifyType.error, true);
                }
            });
        });

        $("a.deletecertificate").live("click", function () {
            $.ajax({
                dataType: "text",
                cache: false,
                type: "POST",
                url: "httphandlers/shoppingcart/deletecertificate.ashx",
                success: function (data) {
                    //getCartFull();
                    //$.advMiniCartRefresh();
                    window.location.reload(true);
                },
                error: function (data) {
                    notify("error deletecertificate" + " status text:" + data.statusText, notifyType.error, true);
                }
            });
        });


        function getCartFull() {
            var trHeader = $("#trCartHeader");
            var cartSummary = $("#tdsummary");
            var cartListHtml = "";
            var products, certificate, cartCountHtml, sum, errorMessage;
            var tpl = "<tr id=\"itemid_{7}\"> \
                            <td class=\"photo\"><a href=\"{0}\">{1}</a></td> \
						    <td> \
						        <div><a href=\"{0}\" class=\"link-pv-name\">{2}</a></div> \
						        <div class=\"sku\">{3}</div> {8} \
						    </td> \
						    <td class=\"price\"><span class=\"price\">{4}</span></td> \
						    <td class=\"count\"><span class=\"input-wrap\" onkeyup=\"defaultButtonClick('recal', event)\"\>{5}</span> \
                                <div class=\"not-available cart-padding\">{9}</div> \
                            </td> \
						    <td class=\"cost\"><span class=\"price\">{6}</span></td> \
						    <td class=\"delete\"><a href=\"javascript:void(0);\" rel=\"{itemId:{7}}\" class=\"cross deleteitem\" title=\"" + localize("shoppingCartDeleteProduct") + "\"></a></td> \
					    </tr>";

            // Если у нас товар под заказ мы используем inputControl, потому что, там нельзя менять количество товара.
            var spinBoxControl = "<input data-plugin=\"spinbox\" type=\"text\" value=\"{0}\" />";
            var inputControl = "<input type=\"text\" value=\"{0}\" readonly='readonly'/>";

            var tplSum = "<div><span class=\"basket-txt\">{0}: </span><span class=\"price\">{1}</span></div>";

            var progress = progressObj.prototype.Init($("#cartWrapper"));

            progress.Show();

            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                url: "httphandlers/shoppingcart/getcart.ashx",
                complete: function () {

                    progress.Hide();
                    if (window.VK && window.VK.callMethod) {
                        VK.init(function () {
                            VK.callMethod("resizeWindow", 827, $("body").height());
                        });
                    }
                },
                success: function (data) {
                    products = data.CartProducts;
                    certificate = data.Certificate;
                    cartCountHtml = data.Count;
                    sum = data.Summary;
                    errorMessage = data.Valid;

                    if (errorMessage != "") {
                        $("#errorMessage").html(errorMessage);
                        $("#aCheckOut").addClass("btn-disabled"); //.attr("onClick", "return false;");

                    }
                    else {
                        $("#aCheckOut").removeClass("btn-disabled"); //.removeAttr("onClick");
                        $("#errorMessage").html("");
                    }

                    if (products.length == 0 && certificate.length == 0) {
                        $("#divEmptyCart").show();
                        $("#shoppingCartTable").hide();
                    }
                    else {
                        $("#divEmptyCart").hide();
                        $("#shoppingCartTable").show();
                    }

                    //generate html list product
                    $.each(products, function (idx, p) {
                        var control = "";

                        // Если у нас товар под заказ мы используем inputControl, потому что, там нельзя менять количество товара.
                        if (p.OrderByRequest == "") {
                            control = String.Format(spinBoxControl, p.Amount);
                        }
                        else {
                            control = String.Format(inputControl, p.Amount);
                        }

                        cartListHtml += String.Format(tpl, p.Link, p.Photo, p.Name, p.SKU, p.Price, control, p.Cost, p.ItemId, p.SelectedOptions, p.Avalible);

                    });

                    //generate html list certificate
                    $.each(certificate, function (idx, p) {
                        cartListHtml += String.Format(tpl, p.Link, p.Photo, p.Name, "", p.Price, p.Amount, p.Cost, p.ItemId, "", "");
                    });

                    $("tr[id^=itemid_]", "#shoppingCartTable").remove();
                    trHeader.after(cartListHtml);

                    //generate html summary
                    cartListHtml = "";

                    for (var i = 0; i < sum.length - 1; i++) {
                        cartListHtml += String.Format(tplSum, sum[i].Key, sum[i].Value);
                    }

                    $("#tdtotal").html(String.Format("<span class=\"basket-txt\">{0}: </span> <span class=\"price\">{1}</span>", sum[sum.length - 1].Key, sum[sum.length - 1].Value));
                    cartSummary.html(cartListHtml);
                    Advantshop.ScriptsManager.Spinbox.prototype.InitTotal();
                },
                error: function () {
                    cartSummary.html(localize("shoppingCartErrorGettingCart"));
                }
            });
        }

        return this.each(function () {
            getCartFull();
        });

    };
})(jQuery);

