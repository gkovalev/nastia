(function ($) {

    var advantshop = Advantshop, scriptManager = advantshop.ScriptsManager, utilities = advantshop.Utilities;

    var spinbox = function (selector, options) {
        this.$obj = advantshop.GetJQueryObject(selector);
        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Spinbox = spinbox;

    spinbox.prototype.InitTotal = function () {
        var objects = $('[data-plugin="spinbox"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            spinbox.prototype.Init(objects.eq(i), utilities.Eval(objects.eq(i).attr('data-spinbox-options')) || {});
        }
    };

    $(spinbox.prototype.InitTotal); // call document.ready

    spinbox.prototype.Init = function (selector, options) {
        var spinboxObj = new spinbox(selector, options);

        spinboxObj.GenerateHtml();
        spinboxObj.BindEvent();

        return spinboxObj;
    };

    spinbox.prototype.GenerateHtml = function () {
        var spinboxObj = this;
        var value$Obj = Number(spinboxObj.$obj.val());
        spinboxObj.$obj.addClass('spinbox');
        var $plus = $('<span />', { 'class': 'spinbox-control spinbox-plus' + (value$Obj === spinboxObj.options.max ? ' spinbox-disabled-plus' : '') });
        var $minus = $('<span />', { 'class': 'spinbox-control spinbox-minus' + (value$Obj === spinboxObj.options.min ? ' spinbox-disabled-minus' : '') });

        spinboxObj.Parts = {};
        spinboxObj.Parts.$plus = $plus;
        spinboxObj.Parts.$minus = $minus;

        var tempCollection = $plus.add($minus);

        spinboxObj.$obj.after(tempCollection);
    };

    spinbox.prototype.BindEvent = function () {
        var spinboxObj = this;

        spinboxObj.$obj.on('keydown.spinbox', function (e) {

            if (e.altKey || e.ctrlKey || e.shiftKey) {
                e.preventDefault();
                return false;
            }

            var code = e.keyCode,
                result = true,
                valOld = spinboxObj.$obj.val();

            //numpad
            if (code > 95 && code < 106) {
                code = code - 48;
            }



            //8 - backspace, 46 - delete

            if (code == 8 || code == 46) {
                result = true;
            } else {
                var symbol = Number(String.fromCharCode(code));
                if (isNaN(symbol) === true) {
                    result = false;
                }
            }

            if (result === true) {
                spinboxObj.valOld = valOld;
            }

            return result;
        });

        spinboxObj.$obj.on('keyup.spinbox', function (e) {
            switch (e.keyCode) {
                case 40:
                    // down arrow
                    spinboxObj.minus(e);
                    break;
                case 38:
                    // up arrow
                    spinboxObj.plus(e);
                    break;
                default:
                    spinboxObj.manual(e);
                    break;
            }
        });

        spinboxObj.$obj.on('mousewheel.spinbox', function (e, delta) {
            var dir = delta > 0 ? 'plus' : 'minus';

            if (dir === 'plus') {
                spinboxObj.plus(e);
            } else {
                spinboxObj.minus(e);
            }

        });

        spinboxObj.Parts.$plus.on('click.spinbox', function (e) {
            spinboxObj.plus(e);
        });

        spinboxObj.Parts.$minus.on('click.spinbox', function (e) {
            spinboxObj.minus(e);
        });
    };

    spinbox.prototype.plus = function (e) {
        var spinboxObj = this;

        spinboxObj.valOld = Number(spinboxObj.$obj.val());

        spinboxObj.valNew = spinboxObj.valOld + spinboxObj.options.step;

        spinboxObj.set(e);
    };

    spinbox.prototype.minus = function (e) {
        var spinboxObj = this;

        spinboxObj.valOld = Number(spinboxObj.$obj.val());

        spinboxObj.valNew = spinboxObj.valOld - spinboxObj.options.step;

        spinboxObj.set(e);

    };

    spinbox.prototype.manual = function (e) {
        var spinboxObj = this;
        spinboxObj.valNew = Number(spinboxObj.$obj.val());
        spinboxObj.set(e);
        return true;
    };

    spinbox.prototype.set = function (e) {
        var spinboxObj = this,
            input = spinboxObj.$obj,
            minus = spinboxObj.Parts.$minus,
            plus = spinboxObj.Parts.$plus,
            valNew = spinboxObj.valNew || Number(input.val()),
            valOld = spinboxObj.valOld || spinboxObj.options.min;

        if (input.val().length === 0) {

            if (e.type === 'click') {
                valNew = spinboxObj.options.min;
            } else {
                return;
            }
        }

        if (isNaN(valNew) === true || valNew < spinboxObj.options.min) {
            valNew = spinboxObj.options.min;
        }

        if (valNew > spinboxObj.options.max) {
            valNew = spinboxObj.options.max;
        }

        plus.removeClass('spinbox-disabled-plus');
        minus.removeClass('spinbox-disabled-minus');

        if (valNew >= spinboxObj.options.max) {
            plus.addClass('spinbox-disabled-plus');
            minus.removeClass('spinbox-disabled-minus');
        }

        if (valNew <= spinboxObj.options.min) {
            plus.removeClass('spinbox-disabled-plus');
            minus.addClass('spinbox-disabled-minus');
        }

        input.val(valNew);
    };

    spinbox.prototype.defaultOptions = {
        min: 0,
        max: Number.POSITIVE_INFINITY,
        step: 1
    };

})(jQuery);