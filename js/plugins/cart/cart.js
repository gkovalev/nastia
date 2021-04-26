(function ($) {

    var advantshop = window.Advantshop,
        scriptManager = advantshop.ScriptsManager,
        utilities = advantshop.Utilities,
        storage = {},
        counter = -1,
        $win = $(window),
        $body = $(document.body);

    var cart = function (selector, options) {
        this.$obj = advantshop.GetJQueryObject(selector);
        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Cart = cart;

    cart.prototype.InitTotal = function () {
        var objects = $('[data-plugin="cart"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            cart.prototype.Init(objects.eq(i), utilities.Eval(objects.eq(i).attr('data-cart-options')) || {});
        }
    };

    $(cart.prototype.InitTotal); // call document.ready

    cart.prototype.Init = function (selector, options) {
        var cartObj = new cart(selector, options);

        cartObj.GenerateHtml();
        cartObj.BindEvent();
        cartObj.SaveInStorage();

        return cartObj;
    };

    cart.prototype.Type = { Full: 'full', Mini: 'mini' };
    cart.prototype.TypeSite = { Default: 'default', Social: 'social' };

    cart.prototype.State = { Create: 'create', Update: 'update', Add: 'add' };

    cart.prototype.GenerateHtml = function (state) {
        var cartObj = this;
        state = state || cart.prototype.State.Create;

        $.ajax({
            dataType: 'json',
            cache: false,
            type: 'POST',
            url: 'httphandlers/shoppingcart/getcart.ashx',
            success: function (data) {
                cartObj.GenerateCallback[cartObj.options.type].call(cartObj, data, state);
            },
            error: function () {
                $('<div />').html(localize("shoppingCartErrorGetingCart"));
            }
        });
    };

    cart.prototype.GenerateCallback = function () {
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Mini] = function (data, state) {
        var cartObj = this;
        var cartDom = cartObj.$obj;

        var progressMini;
        var $list = cartObj.GetListJqueryObj();
        
        if ($list.hasClass('minicart-list-hidden') === true) {
            progressMini = new scriptManager.Progress.prototype.Init($list);
            progressMini.Show();
        }

        var html;

        switch (cartObj.options.typeSite) {
            case cart.prototype.TypeSite.Default:
                html = new EJS({ url: 'js/plugins/cart/templates/minicart.html' }).render(data);
                break;
            case cart.prototype.TypeSite.Social:
                html = new EJS({ url: 'js/plugins/cart/templates/minicart-social.html' }).render(data);
                break;
            default:
                html = new EJS({ url: 'js/plugins/cart/templates/minicart.html' }).render(data);
        }

        cartDom.html(html);

        PIELoad(null, cartDom);

        cartObj.GetListJqueryObj().addClass('minicart-list-hidden');

        if (data.CountNumber === 0) {
            cartDom.addClass('cart-empty');
        } else {
            cartDom.removeClass('cart-empty');
        }

        cart.prototype.GenerateCallback[cart.prototype.Type.Mini][state].call(cartObj, html);
        
        if (progressMini != null) {
            progressMini.Hide();
        }
    };


    cart.prototype.GenerateCallback[cart.prototype.Type.Mini][cart.prototype.State.Create] = function (html) {
        var cartObj = this;
        var cartDom = cartObj.$obj;

        cartDom.addClass(cartObj.options.type + 'cart');
        
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Mini][cart.prototype.State.Add] = function (html) {
        var cartObj = this;
        
        cartObj.ScrollBarInit();
        cartObj.Show();
        cartObj.PosMiniCart();
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Mini][cart.prototype.State.Update] = function (html) {
        var cartObj = this;
        cartObj.Hide();
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Full] = function (data, state) {
        var cartDom = this.$obj;
        
        var progressFull = new scriptManager.Progress.prototype.Init(cartDom);

        progressFull.Show();

        // resize site in vk.com
        if (window.VK && window.VK.callMethod) {
            VK.init(function () {
                VK.callMethod("resizeWindow", 827, $("body").height());
            });
        }

        // $("#aCheckOut") - button go orderconfirmation.aspx
        if (data.Count === 0 || data.Valid.length > 0) {
            $("#aCheckOut").addClass("btn-disabled");
        } else {
            $("#aCheckOut").removeClass("btn-disabled");
        }

        var html = new EJS({ url: 'js/plugins/cart/templates/fullcart.html' }).render(data);
        
        cartDom.html(html);

        PIELoad(null, cartDom);

        cart.prototype.GenerateCallback[cart.prototype.Type.Full][state].call(this, html);

        scriptManager.Spinbox.prototype.InitTotal();
        
        progressFull.Hide();
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Full][cart.prototype.State.Create] = function (html) {
        this.$obj.addClass(this.options.type + 'cart');
    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Full][cart.prototype.State.Update] = function (html) {

    };

    cart.prototype.GenerateCallback[cart.prototype.Type.Full][cart.prototype.State.Add] = function (html) {
    };

    cart.prototype.BindEvent = function () {
        var cartObj = this;
        var options = cartObj.options;

        if (utilities.Events.isExistEvent($body, 'click.cartAdd') != true) {
            $body.on('click.cartAdd', '[data-cart-add]', function (event) {
                event.preventDefault();
                event.stopPropagation();
                cart.prototype.Add(this.getAttribute('data-cart-add'), this.getAttribute('href'), this.getAttribute('data-cart-amount'));
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartRefresh') != true) {
            $body.on('click.cartRefresh', '[data-cart-refresh]', function () {
                cart.prototype.Update();
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartClear') != true) {
            $body.on('click.cartClear', '[data-cart-clear]', function () {
                cart.prototype.Clear();
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartRemoveItem') != true) {
            $body.on('click.cartRemoveItem', '[data-cart-remove]', function () {
                cart.prototype.Remove(this.getAttribute('data-cart-remove'));
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartApplyCertOrCupon') != true) {
            $body.on('click.cartApplyCertOrCupon', '[data-cart-apply-cert-cupon]', function () {
                var code = advantshop.GetJQueryObject(this.getAttribute('data-cart-apply-cert-cupon')).val();
                cart.prototype.ApplyCertCupon(code);
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartRemoveCertificate') != true) {
            $body.on('click.cartRemoveCertificate', '[data-cart-remove-cert]', function () {
                cart.prototype.RemoveCertificate();
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.cartRemoveCupon') != true) {
            $body.on('click.cartRemoveCupon', '[data-cart-remove-cupon]', function () {
                cart.prototype.RemoveCoupon();
            });
        }

        cartObj.BindEventByType[options.type].call(cartObj);
    };

    cart.prototype.BindEventByType = {
        'mini': function () {
            var cartObj = this;
            var cartDom = cartObj.$obj;
            var options = cartObj.options;

            cartDom.on('click.cartClick', function () {
                if (cartDom.hasClass('cart-empty') != true && cartObj.GetListJqueryObj().hasClass('minicart-list-hidden') === true) {
                    cartObj.ScrollBarInit();
                    cartObj.Show();
                }
            });

            cartDom.on('mouseover.cartMouseOver', function () {
                if (cartObj.timer != null) {
                    clearTimeout(cartObj.timer);
                }
            });

            cartDom.on('mouseout.cartMouseOut', function () {
                if (cartObj.timer != null) {
                    clearTimeout(cartObj.timer);
                }

                cartObj.timer = setTimeout(function () { cartObj.Hide.call(cartObj); }, options.timeHide);
            });


            if (options.isHideClickOut === true) {
                $(document.body).on('click.cartClickOut', function (e) {
                    var $list = cartObj.GetListJqueryObj();
                    if ($list.hasClass('minicart-list-hidden') != true && $(e.target).closest(cartDom).length === 0) {
                        cartObj.Hide.call(cartObj);
                    }
                });
            }

            if (options.referencing === true) {
                $win.on('scroll.cartScroll', function () {
                    cartObj.PosMiniCart.call(cartObj);
                });
            }
        },
        'full': function () {

        }
    };

    cart.prototype.Hide = function () {
        var cartObj = this;
        var options = cartObj.options;
        var $list = cartObj.GetListJqueryObj();

        $list[options.animationHide](options.animationSpeed, function() {
            $list.addClass('minicart-list-hidden');
        });

        if (options.type === cart.prototype.Type.Mini && cartObj.timer != null) {
            clearTimeout(cartObj.timer);
        }
    };

    cart.prototype.Show = function () {
        var cartObj = this;
        var options = cartObj.options;
        var $list = cartObj.GetListJqueryObj();
        
        $list.removeClass('minicart-list-hidden').hide();
        $list[options.animationShow](options.animationSpeed);

        if (options.type === cart.prototype.Type.Mini) {

            if (cartObj.timer != null) {
                clearTimeout(cartObj.timer);
            }

            cartObj.timer = setTimeout(function () { cartObj.Hide.call(cartObj); }, options.timeHide);
        }
    };

    cart.prototype.PosMiniCart = function () {
        var cartObj = this;
        var $list = cartObj.GetListJqueryObj();
        var currentScroll = $win.scrollTop();

        cartObj.positionDefault = cartObj.positionDefault || {left: cartObj.$obj.offset().left - ($list.outerWidth() - cartObj.$obj.outerWidth()), top:cartObj.$obj.outerHeight()};

        /*if ($list.hasClass('minicart-list-hidden') != true) {*/
            if (currentScroll > cartObj.positionDefault.top) {
                $list.addClass('minicart-fixed');
                $list.css({ left: cartObj.positionDefault.left });
            } else {
                $list.removeClass('minicart-fixed');
                $list.css({ left: '' });
            }
        /*}*/

    };

    cart.prototype.Add = function (productId, url, quantity) {

        var amount = quantity != "" ? parseInt(quantity) : 1;

        var productData = {
            productID: productId,
            amount: $("#txtAmount").length > 0 ? $("#txtAmount").val() : amount,
            AttributesXml: $("#customOptionsHidden_" + productId).length > 0 ? $("#customOptionsHidden_" + productId).val() : null
        };

        $.ajax({
            url: "httphandlers/shoppingcart/addtocart.ashx",
            cache: false,
            type: "POST",
            data: productData,
            success: function (data) {
                if (data === 'success') {
                    for (var type in cart.prototype.GetAllItemsInStorage()) {
                        for (var item in storage[type]) {
                            storage[type][item].GenerateHtml(cart.prototype.State.Add);
                        }
                    }
                } else if (data === 'redirect') {
                    window.location.href = url;
                } else if (data === 'fail') {
                    throw (localize('errorAddingToCart'));
                }
            },
            error: function () {
                throw (localize("errorAddingToCart"));
            },
        });
    };

    cart.prototype.Update = function () {
        var cartObj = this;
        var counters = $('[data-cart-itemcount]');
        var listProducts = "";
        var itemId;

        for (var i = 0, arrLength = counters.length; i < arrLength; i++) {
            itemId = counters.eq(i).closest('tr').attr('data-itemid');
            listProducts += itemId + "_" + counters.eq(i).val() + ";";
        }

        $.ajax({
            dataType: "text",
            cache: false,
            data: { list: listProducts },
            type: "POST",
            url: "httphandlers/shoppingcart/updatecart.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                alert("error recalc");
            }
        });
    };

    cart.prototype.Clear = function () {
        var cartObj = this;

        $.ajax({
            dataType: "text",
            cache: false,
            type: "POST",
            url: "httphandlers/shoppingcart/clearcart.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                notify("error clearcart" + " status text:" + data.statusText, notifyType.error, true);
            }
        });
    };

    cart.prototype.Remove = function (itemId) {
        var cartObj = this;

        $.ajax({
            dataType: "text",
            cache: false,
            data: { 'itemid': itemId },
            type: "POST",
            url: "httphandlers/shoppingcart/deletefromcart.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                alert("error delete");
            }
        });
    };

    cart.prototype.ApplyCertCupon = function (code) {
        var cartObj = this;

        $.ajax({
            dataType: "text",
            cache: false,
            data: { 'code': code },
            type: "POST",
            url: "httphandlers/shoppingcart/applycertorcupon.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                alert("error delete");
            }
        });
    };

    cart.prototype.RemoveCertificate = function () {
        var cartObj = this;

        $.ajax({
            dataType: "text",
            cache: false,
            type: "POST",
            url: "httphandlers/shoppingcart/deletecertificate.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                alert("error delete");
            }
        });
    };

    cart.prototype.RemoveCoupon = function () {
        var cartObj = this;

        $.ajax({
            dataType: "text",
            cache: false,
            type: "POST",
            url: "httphandlers/shoppingcart/deleteCoupon.ashx",
            success: function (data) {
                for (var type in cart.prototype.GetAllItemsInStorage()) {
                    for (var item in storage[type]) {
                        storage[type][item].GenerateHtml(cart.prototype.State.Update);
                    }
                }
            },
            error: function (data) {
                alert("error delete");
            }
        });
    };

    cart.prototype.SaveInStorage = function () {
        var cartObj = this;
        var cartDom = cartObj.$obj;
        var id;

        if (cartDom.attr('id')) {
            id = cartDom.attr('id');
        } else {
            id = 'cart-id-' + (counter += 1);
            cartDom.attr('id', id);
        }

        storage[cartObj.options.type] = storage[cartObj.options.type] || {};

        storage[cartObj.options.type][id] = this;

    };

    cart.prototype.GetAllItemsInStorage = function () {
        return storage;
    };

    cart.prototype.GetAllItemsByTypeInStorage = function (type) {
        return storage[type] || null;
    };

    cart.prototype.ScrollBarInit = function () {

        if ($.fn.jScrollPane == null) {
            return;
        }

        var cartObj = this;
        var $scrollDom = cartObj.$obj.find('[data-plugin="scrollbar"]');
  
        var height = $scrollDom.outerHeight();
        var scrollbarOpts = utilities.Eval($scrollDom.attr('data-cart-scrollbar'));
        
        if (height >= scrollbarOpts.height) {
            $scrollDom.css('height', scrollbarOpts.height);
            $scrollDom.jScrollPane();
        } else if ($scrollDom.data('jsp') != null) {
            $scrollDom.css('height', 'auto');
            $scrollDom.data('jsp').destroy();
        }
    };

    cart.prototype.GetListJqueryObj = function() {
        return this.$obj.find('.minicart-list');
    };
    
    cart.prototype.defaultOptions = {
        timeHide: 5000,
        animationShow: 'fadeIn',
        animationHide: 'fadeOut',
        animationSpeed: 700,
        referencing: true,
        isHideClickOut: true,
        type: cart.prototype.Type.Full,
        typeSite: cart.prototype.TypeSite.Default
    };

})(jQuery);