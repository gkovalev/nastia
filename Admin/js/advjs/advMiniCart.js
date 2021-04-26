(function ($) {



    jQuery.fn.advMiniCart = function (o) {
        var progressObj = Advantshop.ScriptsManager.Progress;


        var options = jQuery.extend({
            control: ".top-panel-cart",
            classShow: "visibleCart",
            animate: "fade", //slide, fade, none
            speedAnimate: 700,
            referencing: true,
            speedMove: 300,
            timerHide: 5000,
            isHideClickOut: true
        }, o);

        //public api      
        jQuery.extend(this, {
            showCart: function (animate) { showCart.apply(this, [animate]); },
            closeCart: function (animate) { closeCart.apply(this, [animate]); },
            addToCart: function (id) {
                addToCart.apply(this, [id]);
            }
        });

        $("a[rel^=productid]").click(function (event) {
            var rel = $(this).attr("rel").replace(/\s*/g, '');
            var id = /productid:(\d+)/.exec(rel)[1];
            var amount = 1;
            if ($("#txtAmount").length) {
                amount = parseInt($("#txtAmount").val()) || 1;
            }

            var customOptions = "";
            if ($("#customOptionsHidden_" + id).length) {
                customOptions = $("#customOptionsHidden_" + id).val();
            }

            addToCart(id, amount, customOptions, event);

        });

        function showCart(animate) {
            var cart = $("#cart-modal");

            var progress = progressObj.prototype.Init(cart);

           progress.Show();

            animate = animate != null ? animate : options.animate;
            switch (animate) {
                case "fade":
                    cart.stop(true, true).fadeIn(options.speedAnimate, function () { posCart.call(cart); refreshScroll(cart); progress.Hide(); });
                    break;
                case "slide":
                    cart.stop(true, true).slideDown(options.speedAnimate, function () { posCart.call(cart); refreshScroll(cart); progress.Hide(); });
                    break;
                case "none":
                    cart.stop(true, true).addClass(options.classShow);
                    posCart.call(cart);
                    refreshScroll(cart);
                    break;
                default:
                    cart.stop(true, true).addClass(options.classShow);
                    posCart.call(cart);
                    refreshScroll(cart);
                    break;
            }

        }

        function posCart() {
            var cart = $("#cart-modal"); ;
            var positionCartDefault = parseInt(cart.data("offsetTop"));
            if (cart.is(":visible")) {
                var scrollTop = jQuery(window).scrollTop();
                var cartPosition = cart.offset();
                if (scrollTop > positionCartDefault) {
                    cart.css("left", cartPosition.left).addClass("cart-fixed");
                } else {
                    cart.css("left", "").removeClass("cart-fixed");
                }
            }
        }

        function closeCart(animate) {
            var cart = $("#cart-modal");
            animate = animate != null ? animate : options.animate;
            switch (animate) {
                case "fade":
                    cart.stop(true, true).fadeOut(options.speedAnimate);
                    break;
                case "slide":
                    cart.stop(true, true).slideUp(options.speedAnimate);
                    break;
                case "none":
                    cart.stop(true, true).removeClass(options.classShow);
                    break;
                default:
                    cart.stop(true, true).removeClass(options.classShow);
                    break;
            }
        }

        function addToCart(productId, amount, customOptions, event) {
            $.ajax({
                url: "httphandlers/shoppingcart/addtocart.ashx",
                cache: false,
                type: "POST",
                async: false,
                data: { productID: productId, amount: amount, AttributesXml: customOptions },
                success: function (data) {
                    if (data == "success") {
                        getCartMini(showCart);

                        event = event || window.event;
                        event.preventDefault();
                    } else if (data == "fail") {
                        throw (localize("errorAddingToCart"));
                    }
                },
                error: function () {
                    throw (localize("errorAddingToCart"));
                }
            });
        }

        function getCartMini(func) {
            $.advMiniCartRefresh(func);
        }

        function refreshScroll(cart) {
            //scrollbar
            var cartList = cart.find("#cart-list");
            //var heightInit = parseInt(cartList.css("max-height"));
            var heightInit = 350;

            if (cartList.outerHeight() > heightInit) {
                if (!cartList.data("scrollbar") || !cartList.find(".scrollbar").length) {
                    var scrollbar = cartList.tinyscrollbar({ heightInit: heightInit });
                    cartList.data("scrollbar", scrollbar);
                }
                else {
                    cartList.data("scrollbar").tinyscrollbar_update();
                }
            }
        }

        return this.each(function () {
            var cart = jQuery(this),
                control = jQuery(options.control),
                timerHide;

            control.click(function (e) {
                e.cancelBubble = true;
                e.stopPropagation();

                if ($(e.target).closest("#cart-modal").length || !cart.find("div.cart-list > *").length) return;

                if (cart.is(":hidden")) {
                    showCart.apply(cart, [options.animate]);
                    if (options.timerHide) {
                        if (timerHide) clearTimeout(timerHide);
                        timerHide = setTimeout(function () { closeCart.apply(cart, [options.animate]); }, options.timerHide);
                    }
                } else
                    closeCart.apply(cart, [options.animate]);
                return;
            });

            control.mouseover(function () {
                if (timerHide) clearTimeout(timerHide);
                return false;
            });

            control.mouseleave(function () {
                if (cart.is(":visible") && options.timerHide) {
                    if (timerHide) clearTimeout(timerHide);
                    timerHide = setTimeout(function () { closeCart.apply(cart, [options.animate]); }, options.timerHide);
                }
                return false;
            });

            if (options.isHideClickOut) {
                $("body").click(function (e) {
                    if (cart.is(":visible") && !$(e.target).closest("#cart-modal").length && !$(e.target).closest("a[rel^=productid]").length)
                        closeCart.apply(cart, [options.animate]);
                });
            }

            cart.data("offsetTop", cart.css("top"));

            if (options.referencing) {
                jQuery(window).scroll(function () { posCart.call(cart); });
            }

            getCartMini();
        });

    };

    jQuery.extend({
        advMiniCartRefresh: function (func) {
            var cart = $("#cart-modal");
            var cartList = $("#cart-list", cart);
            var cartSummary = $("#summary-body", cart);
            var cartCount = $("#cart-count");
            var cartListHtml = "";
            var products, certificate, cartCountHtml, sum;
            var tpl = "<div><div class=\"p-img\"><a href=\"{0}\" class=\"link-p-name\">{1}</a></div> \
                                    <div class=\"p-info\"> \
                                        <div><a href=\"{0}\" class=\"link-p-name\">{2}</a></div> \
                                        <div><span class=\"font-param\">{5}:</span> {3}</div> \
                                        <div><span class=\"font-param\">{6}:</span> {4}</div> \
                                    </div></div>";
            var tplSum = "<div class=\"sum\">{0}: {1}</div>";
            $.ajax({
                dataType: "json",
                cache: false,
                type: "POST",
                url: "httphandlers/shoppingcart/getcart.ashx",
                success: function (data) {
                    products = data[0];
                    certificate = data[1];
                    cartCountHtml = data[2];
                    sum = data[3];

                    //generate html list product
                    $.each(products, function (idx, p) {
                        cartListHtml += String.Format(tpl, p.Link, p.Photo, p.Name, p.Amount, p.Price, localize("shoppingCartAmount"), localize("shoppingCartPrice"));
                    });
                    //generate html list certificate
                    $.each(certificate, function (idx, p) {
                        cartListHtml += String.Format(tpl, p.Link, p.Photo, p.Name, p.Amount, p.Price, localize("shoppingCartAmount"), localize("shoppingCartPrice"));
                    });
                    cartList.html(cartListHtml);

                    //generate html summary
                    cartListHtml = "";

                    for (var i = 0; i < sum.length; i++) {
                        cartListHtml += String.Format(tplSum, sum[i].Key, sum[i].Value);
                    }

                    cartSummary.html(cartListHtml);

                    //paste cart count
                    if (cartCount) cartCount.html(cartCountHtml);

                    if (func) {
                        func();
                    }

                },
                error: function () {
                    cartList.html(localize("shoppingCartErrorGetingCart"));
                    cartSummary.empty();
                }
            });
        }
    });
})(jQuery);

