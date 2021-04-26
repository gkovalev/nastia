$(function () {

    if (window.DisplayFilter) {
        DisplayFilter(10, 5);
    }

    $("#brand-filter input, #property-filter input").click(function () {
        ApplyFilter(this, false, true, true);
    });
    $("#property-filter input, #property-filter input").click(function () {
        ApplyFilter(this, false, true, true);
    });

    $("#ddlCategory").change(function () {
        ApplyFilter(this, false, true, true);
    });
    

    if ($("a.zoom span.label-p").length) {
        $("a.zoom span.label-p").click(function () {
            return false;
        });
    }


    $("a.btn.btn-disabled").live("click", function (e) {
        e = e || window.event;
        e.preventDefault();
        return false;
    });


    if ($("input[placeholder], textarea[placeholder]").length)
        $("input[placeholder], textarea[placeholder]").placeholder();


    $('div[id^=rating_]').each(function () {
        $(this).raty({
            hints: [localize("ratingAwfull"), localize("ratingBad"), localize("ratingNormal"), localize("ratingGood"), localize("ratingExcelent")],
            readOnly: $(this).hasClass("rating-readonly"),
            score: parseFloat($(this).next("input:hidden[id^=rating_hidden_]").val()),
            click: function (score, evt) {
                var newscore = VoteRating($(this).attr('id').replace("rating_", ""), score);
                if (newscore != 0) {
                    $(this).raty('score', newscore);
                } else {
                    $(this).raty('score', $(this).next("input:hidden[id^=rating_hidden_]").val());
                    notify(localize("ratingAlreadyVote"), notifyType.notify);
                }
                $(this).raty('readOnly', true);
            }
        });
    });


    if ($("ul.carousel-product:visible").length)
        $("ul.carousel-product:visible").jcarousel({ scroll: 1 });

    if ($("div.carousel-small ul").length) {
        $("div.carousel-small").jcarousel({ scroll: 1, visible: 4 });
    }


    if ($("a[data-plugin=fancybox]").length) {
        $("a[data-plugin=fancybox]").fancybox();
    }

    if ($("a.fancybox[rel=fancybox-gallery]", "div.zoom-c").length) {
        $("a.fancybox[rel=fancybox-gallery]", "div.zoom-c").fancybox({
            'transitionIn': 'none',
            'transitionOut': 'none',
            'titlePosition': 'over',
            'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
                return '<span id="fancybox-title-over">Image ' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? '<br /><br />' + title : '') + '</span>';
            },
            'onComplete': function () {
                if ($.browser.msie && $.browser.version < 9) {
                    $('body').addClass('fancybox-videos-hide');
                }

            },
            'onClosed': function () {
                if ($.browser.msie && $.browser.version < 9) {
                    $('body').removeClass('fancybox-videos-hide');
                }
            }
        });
    }

    $("#icon-zoom, #link-fancybox", "div.zoom-c").click(function () {
        var listFancybox = $("div.fancybox-list a", "div.zoom-c");
        var lis = $("li", "#carouselDetails");
        var liSel = lis.filter(".selected");
        var idx = lis.index(liSel);

        if (idx == -1) idx = 0;

        $(listFancybox[idx]).click();

        return false;
    });

    if ($("#carousel-preview").length) {
        $("a", "#carousel-preview").click(function (e) {
            var li = $(this).closest("li");
            li.siblings("li.selected").removeClass("selected");
            li.addClass("selected");
            return false;
        });
    }


    //    if ($(".tabs").length) {
    //        $(".tabs").advTabs({ callbackOpen: function () { initValidation($("form")); }, contentPlaceholderLink: "#tabs-link" });
    //    }

    if ($("div.slider").length) {

        var min = parseInt($(this).find("span.min").text());
        var max = parseInt($(this).find("span.max").text());
        var curMin = parseInt($("#sliderCurentMin").val());
        var curMax = parseInt($("#sliderCurentMax").val());

        if (isNaN(min)) min = 0;
        if (isNaN(max)) max = 0;
        if (isNaN(curMin)) curMin = 0;
        if (isNaN(curMax)) curMax = 0;

        var slider = $("div.slider");

        slider.data("prices", { from: curMin, to: curMax });



        slider.slider({
            range: true,
            min: min,
            max: max,
            values: [curMin, curMax],
            slide: function (event, ui) { sladeMove.call(this, ui); makeMagic($(this)); },
            change: function (event, ui) { sladeMove.call(this, ui); ApplyFilter(this, false, true, false); }

        });

        function sladeMove(ui) {
            $(this).find("span.min").text(ui.values[0]);
            $(this).find("span.max").text(ui.values[1]);
        }

        $("div.slider").each(function () {
            $(this).find("span.min").text($(this).slider("option", "values")[0]);
            $(this).find("span.max").text($(this).slider("option", "values")[1]);
        });

        makeMagic(slider);

    }

    if ($("table.avangard").length) {
        var rbs = $("table.avangard input:radio");
        var trs = $("table.avangard tr:not(.header)");
        $("table.avangard").click(function (e) {
            var tr = $(e.target).closest("tr:not(.header,.selected)");

            if (!tr.length) return;

            trs.removeClass("selected");
            tr.addClass("selected");

            var rb = tr.find("input:radio");
            if (!rb.length) return;
            rbs.removeAttr("checked");
            rb.attr("checked", "checked");
        });
    }

    if ($("input:checkbox.adress-payment").length) {
        $("input:checkbox.adress-payment").live("click", function () {
            var listP = $(this).closest("li").find("div.adress-payment");

            if (!listP.length) return false;

            listP.slideToggle();
            return true;
        });
    };

    if ($("table.pv-table").length) {
        var el, cell, tooltip, tooltipImg, tooltipPosition, path;
        var wrapperTooltip = "<div class='tooltip' id='pv-table-tooltip'><img src=\".\" /><div class='declare'></div></div>";

        $("table.pv-table tr:not(.head)").mouseenter(function (e) {

            cell = $(e.target).closest("tr").children("td.icon[abbr]");

            if (!cell.length) return;

            el = cell.find("div.photo");
            path = cell.attr("abbr");

            if (!path) return;

            if (!$("#pv-table-tooltip").length)
                $("body").append(wrapperTooltip);

            tooltip = $("#pv-table-tooltip");

            tooltipImg = tooltip.find("img").attr("src", path);

            tooltip.append(tooltipImg);

            tooltipImg.load(function () {

                tooltip.show();

                tooltipPosition = el.offset();

                tooltip.css({
                    top: tooltipPosition.top - 47,
                    left: tooltipPosition.left - (tooltip.outerWidth() + 13)
                });

                PIELoad(tooltip);
            });
        });

        $("table.pv-table tr:not(.head)").mouseleave(function () {
            tooltip = $("#pv-table-tooltip");
            if (tooltip && tooltip.is(":visible")) {
                tooltip.hide();
            }
        });
    }

    $("#check-status a.btn").click(function () {
        var number = $("#check-status input").val();
        if (number.length) {
            var status = CheckOrder(htmlEncode(number.substr(0, 20)));
            if (status) {
                $("#orderStatus").text(localize("checkOrderState") + ":" + status.StatusName);
                if (status.StatusComment) {
                    $("#orderStatus").append("<br />" + localize("checkOrderComent") + ": " + status.StatusComment);
                }
                $("#orderStatus").show();
            } else {
                throw Error(localize("checkOrderError"));
            }
        }
        return false;
    });

    $(".tree-item:not(.tree-item-nosubcat), .tree-item-selected:not(.tree-item-nosubcat) ").hover(function (e) {
        var target = $(this);

        var submenu = target.find(".tree-submenu");
        if (!submenu.length) return true;

        submenu.removeClass("submenu-orientation").css({ left: "" });

        submenu.show();

        var bWidth = $("body").width();
        var sPos = submenu.offset().left;
        var sWidth = submenu.outerWidth();
        var result = sPos + sWidth;

        if (bWidth <= sWidth) {
            submenu.css({ left: -sPos });
        } else if (bWidth <= result) {
            //submenu.addClass("submenu-orientation");
            submenu.css({ left: -(result - bWidth) });
        } else {
            submenu.removeClass("submenu-orientation").css({ left: "" });
        }

    },
        function (e) {
            var target = $(this);
            var submenu = target.find(".tree-submenu");

            if (!submenu.length) return true;

            setTimeout(function () {
                submenu.hide();
            }, 1);

        });

    $("table.categories").click(function (e) {

        e.cancelBubble = true;
        var target = $(e.target);

        if (!target.is("td:not(cat-split)")) return true;

        var link = target.find("a").attr("href");

        if (!link || !link.length) return true;

        window.location.href = $("base").attr("href") + link;
    });

    var isCtrl = false;

    $(document).keyup(function (e) {
        if (e.keyCode == 17)
            isCtrl = false;
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 17)
            isCtrl = true;

        //left arrow
        if (e.keyCode == 37 && isCtrl == true) {
            if ($("#paging-prev").length)
                document.location = $("#paging-prev").attr("href");
        }

        //right arrow
        if (e.keyCode == 39 && isCtrl == true) {
            if ($("#paging-next").length)
                document.location = $("#paging-next").attr("href");
        }
    });

    if ($(".btn-disabled").length) {
        var btn = $(".btn-disabled");
        if (btn.attr("onClick")) {
            btn.attr("onClickOld", btn.attr("onClick")).attr("onClick", "return false;");
        }
    }

    if ($("input.autocompleteRegion").length) {
        $("input.autocompleteRegion").autocomplete("HttpHandlers/GetRegions.ashx", {
            delay: 10,
            minChars: 1,
            matchSubset: 1,
            autoFill: true,
            matchContains: 1,
            cacheLength: 10,
            selectFirst: true,
            //formatItem: liFormat,
            maxItemsToShow: 10
            //onItemSelect: selectItem
        });
    }

    if ($("input.autocompleteCity").length) {
        $("input.autocompleteCity").autocomplete('HttpHandlers/GetCities.ashx', {
            delay: 10,
            minChars: 1,
            matchSubset: 1,
            autoFill: true,
            matchContains: 1,
            cacheLength: 10,
            selectFirst: true,
            //formatItem: liFormat,
            maxItemsToShow: 10
            //onItemSelect: selectItem
        });
    }

    if ($("input.autocompleteSearch").length) {
        $("input.autocompleteSearch").autocomplete('HttpHandlers/GetSearch.ashx', {
            delay: 10,
            minChars: 1,
            matchSubset: 1,
            autoFill: false,
            matchContains: 1,
            cacheLength: null,
            selectFirst: false,
            //formatItem: liFormat,
            maxItemsToShow: 10
            //onItemSelect: selectItem
        });
    }


    if ($("a.trialAdmin").length) {
        $.advModal({
            title: localize("demoMode"),
            control: $("a.trialAdmin"),
            isEnableBackground: true,
            htmlContent: localize("demoCreateTrial"),
            buttons: [
                { textBtn: localize("demoCreateNow"), isBtnClose: true, classBtn: "btn-confirm", func: function () { window.location = localize("trialUrl"); } },
                { textBtn: localize("demoCancel"), isBtnClose: true, classBtn: "btn-action" }
            ]
        });
    }

});

$(window).load(function () {
    if ($('#flexslider').length) {
        var paramsCustom = $("#carouselOptions").length ? $("#carouselOptions").metadata({ type: 'attr', name: 'value' }) : {};

        $('#flexslider').flexslider(paramsCustom);

        PIELoad('.flex-control-nav a');
    }

    if (window.notify) {
        notify(null, null, null, { showContainer: true });
    }

    PIELoad();
});

