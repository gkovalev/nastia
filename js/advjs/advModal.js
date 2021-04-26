(function (jQuery) {
    jQuery.extend({
        advModal: function (o) {
            var options = jQuery.extend({
                modalClass: null,
                modalId: null,
                title: null,
                htmlContent: null,
                buttons: null,
                isEnableBackground: true,
                opacityBackground: 0.4,
                opacityModal: 1,
                control: null,
                controlEvent: "click",
                clickOut: true,
                cross: true,
                closeEsc: true,
                enableTouch: true,
                positionShow: true,
                controlBeforeOpen: null,
                controlAfterOpen: null,
                funcCross: null,
                beforeOpen: null,
                afterOpen: null,
                beforeClose: null,
                afterClose: null,
                hideError: true,
                initCallback: null
            }, o);

            var modal = $("<div>").addClass("modal"), modalBackground = $("<div>").addClass("modal-background");

            if (options.isEnableBackground && !$("div.modal-background").length)
                $("form").append(modalBackground);

            if (options.cross)
                modal.append("<div class='modal-close cross'></div>");

            if (options.title)
                modal.append("<div class='title'></div>");

            if (options.htmlContent)
                modal.append("<div class='content'></div>");

            if (options.buttons)
                modal.append("<div class='btns'></div>");

            $("form").append(modal);

            if (options.modalClass) modal.addClass(options.modalClass);
            if (options.modalId) modal.attr("id", options.modalId);

            if (options.isEnableBackground && !modalBackground.length)
                modalBackground = $("div.modal-background");

            if (options.isEnableBackground) {
                modalBackground = modal.prev("div.modal-background");
                modalBackground.css({ opacity: options.opacityBackground });
            }

            var title = modal.children("div.title"),
					content = modal.children("div.content"),
					btnsContainer = modal.children("div.btns");


            //title
            if (title.length)
                title.append(options.title);
            //content
            if (content.length)
                content.append(options.htmlContent);

            //generate btn
            if (btnsContainer.length) {
                var htmlBtns = "";
                $.each(options.buttons, function (idx, el) {
                    htmlBtns += "<a href='#' class='btn btn-middle btn-modal ";
                    if (el.classBtn)
                        htmlBtns += el.classBtn + "'>";
                    else
                        htmlBtns += "'>";
                    if (el.textBtn)
                        htmlBtns += el.textBtn + "</a>";
                });
                btnsContainer.html(htmlBtns);
            }

            //bindEvent
            if (btnsContainer.length) {
                var btns = btnsContainer.children("a.btn");
                btns.each(function (idx, el) {
                    if (options.buttons[idx].func != null)
                        $(el).click(options.buttons[idx].func);
                    if (options.buttons[idx].isBtnClose == true)
                        $(el).click(function () { modalClose.apply(modal); return false; });
                });
            }
            if (options.control) {
                var elem = $(options.control);
                elem.live(options.controlEvent, function (event) {
                    if (modal.is(":hidden")) {
                        if (options.controlBeforeOpen)
                            (options.controlBeforeOpen).apply(modal);

                        modalShow.apply(modal, [null, event]);

                        if (options.controlAfterOpen)
                            (options.controlAfterOpen).apply(modal);
                    }
                });

            }

            if (options.clickOut) {
                $("body").click(function (e) {
                    var isModalOnClick = $(e.target).closest(modal).length;

                    if (!isModalOnClick && modal.is(":visible")) {
                        modalClose.apply(modal);
                    }

                });
            }
            if (options.closeEsc) {
                $(document).keyup(function (e) {
                    if (modal.is(":visible") && e.keyCode == 27) {
                        modalClose.apply(modal);
                    }
                });
            }


            modal.find("div.modal-close").click(function () {
                modalClose.apply(modal);
                if (options.funcCross)
                    (options.funcCross).apply(modal);
                return false;
            });
            $(window).resize(
					function () {
					    if (modal.is(":visible")) modalPosition.apply(modal);
					}
				);

            //position modal	
            modalPosition.apply(modal);

            if (window.jQuery.IsTouchDevice && jQuery.IsTouchDevice()) {
                var modalOffset, docHeight;

                modal.addClass("m-touch");

                if (modalBackground.length > 0)
                    modalBackground.addClass("mb-touch");

                $(window).scroll(function () {
                    if (!docHeight) modalBackground.css({ height: $(document).height() });
                    if (!modalOffset) modalOffset = modal.offset();
                    modal.css({ top: $(window).scrollTop() + modalOffset.top });
                });
            }
            //api
            $.extend(modal, {
                modalShow: function (callback) {
                    modalShow.apply(modal, [callback]);
                },
                modalClose: function (callback) {
                    modalClose.apply(modal, [callback]);
                },
                modalTitle: function (content, callback) {
                    modalTitle.apply(modal, [content, callback]);
                },
                modalContent: function (content, callback) {
                    modalContent.apply(modal, [content, callback]);
                },
                modalBtns: function (content, callback) {
                    modalBtns.apply(modal, [content, callback]);
                },
                modalPosition: function (callback) {
                    modalPosition.apply(modal, [callback]);
                }
            });

            if (options.initCallback) {
                options.initCallback();
            }

            return modal;
            //functions
            function modalClose(callback) {
                var modalPrivate = $(this);
                if (modalPrivate.is(":visible")) {
                    if (options.beforeClose) (options.beforeClose).apply(modalPrivate, [modalPrivate]);

                    if (options.isEnableBackground)
                        modalPrivate.prevAll("div.modal-background").hide();

                    PIEDeatch($(".btns a.btn, input, textarea, .pie"));

                    modal.hide();

                    if (callback) callback.apply(modalPrivate);
                }
            }
            function modalShow(callback, event) {
                event = event || window.event;
                if (event) {
                    if (event.stopPropagation) {
                        event.stopPropagation();
                    } else {
                        event.cancelBubble = true;
                    }
                }

                var modalPrivate = $(this);

                if (modalPrivate.is(":hidden")) {
                    if (options.beforeOpen) (options.beforeOpen).apply(modalPrivate, [modalPrivate]);

                    if (options.isEnableBackground)
                        modalPrivate.prevAll("div.modal-background").show();


                    if (options.hideError) {
                        var elems = $("input,select,textrea").not(":submit, :reset, :image, [disabled]").filter(function () {
                            return (this.className.indexOf("valid-") != -1);
                        });
                        elems.nextAll(".error-message").hide();
                    }

                    modal.show();

                    if (options.positionShow) modalPosition.apply(modalPrivate);

                    if (options.afterOpen) {
                        (options.afterOpen).apply(modalPrivate, [modalPrivate]);
                    }

                    if (callback) callback.apply(modalPrivate);


                    if ($.browser.msie && $.browser.version == 7 && $("div.btns", modalPrivate).length)//fix ie7
                    {
                        $("div.btns", modalPrivate).css("width", $("div.content", modalPrivate).width());
                    }

                    PIELoad($(".btns a.btn, input, textarea, .pie"));
                }
            }
            function modalTitle(newContent, callback) {

                if (!newContent) newContent = " ";

                var modalPrivate = $(this);

                if (!title.length) {
                    modalPrivate.prepend("<div class='title'></div>");
                    title = modalPrivate.children("div.title");
                }

                title.html(newContent);

                if (callback)
                    callback.apply(modalPrivate, [title]);
            }
            function modalContent(newContent, callback) {

                if (!newContent) newContent = " ";

                var modalPrivate = $(this);

                if (!content.length) {
                    if (title.length)
                        title.after("<div class='content'></div>");
                    else
                        modalPrivate.append("<div class='content'></div>");

                    content = modalPrivate.children("div.content");
                }

                content.html(newContent);

                if (callback) callback.apply(modalPrivate, [content]);
            }
            function modalBtns(arrayBtn) {
                var modalPrivate = $(this);
                var btnsContainer = modalPrivate.children("div.btns");

                if (!arrayBtn && btnsContainer.length) btnsContainer.empty();


                if (!btnsContainer.length) {
                    modalPrivate.append("<div class='btns'></div>");
                    btnsContainer = modalPrivate.children("div.btns");
                }
                else {
                    btnsContainer.empty();
                }



                var htmlBtns = "";

                $.each(arrayBtn, function (idx, el) {
                    htmlBtns += "<a href='#' class='btn  btn-modal ";
                    if (el.classBtn)
                        htmlBtns += el.classBtn + "'>";
                    else
                        htmlBtns += "'>";
                    if (el.textBtn)
                        htmlBtns += el.textBtn + "</a>";
                });

                btnsContainer.html(htmlBtns);

                var btns = btnsContainer.children("a.btn");
                btns.each(function (idx, el) {
                    if (arrayBtn[idx].func != null)
                        $(el).click(arrayBtn[idx].func);
                    if (arrayBtn[idx].isBtnClose == true)
                        $(el).click(function () { modalClose.apply(modalPrivate); });
                });
            }
            function modalPosition(callback) {

                var bodyWidth = $(window).width(),
						bodyHeight = $(window).height();

                var modalPrivate = $(this);

                var widthModal = modalPrivate.outerWidth(),
					    heightModal = modalPrivate.outerHeight();

                modal.css({ left: (bodyWidth - widthModal) / 2,
                    top: (bodyHeight - heightModal) / 2,
                    opacity: options.opacityModal
                });

                if (callback) callback.apply(modalPrivate);

            }
        }
    })
})(jQuery);


