(function (jQuery) {
    jQuery.fn.advReviews = function (o) {
        var options = jQuery.extend({
            entityId: null,
            entityType: null,
            maxlevel: 5
        }, o);

        var itemHtml = "<div class=\"review-item\"><div class=\"author\">{0}<span class=\"date\">{1}</span></div><div class=\"message\">{2}</div><div class=\"btn-review\"><a id=\"btn-send\" href=\"javascript:void(0);\" rel=\"{3}\">{6}</a> {5}</div>{4}</div>";


        var sendBlock = "<form id=\"{0}\" novalidate=\"novalidate\">\
                            <ul class=\"form reviews-form\"><li class=\"header\">{7}</li> \
                            <li class=\"reviews-form-name\"><div class=\"param-name\"><label for=\"txtName-reply\">{2}</label></div><div class=\"param-value\"><input type=\"text\" class=\"valid-required\" id=\"txtName-reply\" name=\"txtName-reply\" maxlemgth=\"150\"></div></li>\
                            <li class=\"reviews-form-email\"><div class=\"param-name\"><label for=\"txtEmail-reply\">{3}</label></div><div class=\"param-value\"><input type=\"text\" class=\"valid-email\" id=\"txtEmail-reply\"  name=\"txtEmail-reply\" maxlemgth=\"150\"></div></li> \
                            <li class=\"clear\"></li> \
                            <li class=\"reviews-form-message li-long\"><div class=\"param-name\"><label for=\"txtMessage-reply\">{4}:</label></div><div class=\"param-value\"><textarea class=\"valid-required\" id=\"txtMessage-reply\" name=\"txtMessage-reply\" cols=\"20\" rows=\"2\"></textarea></div></li> \
                            <li class=\"reviews-send footer\"> \
                                <span class=\"btn-c\"><a id=\"btn-send\" class=\"btn btn-submit btn-middle\" href=\"javascript:void(0);\" rel=\"{1}\">{5}</a></span> \
                                <span class=\"btn-c\"><a id=\"btn-cancel\" class=\"btn btn-action btn-middle\" href=\"javascript:void(0);\">{6}</a></span> \
                            </li> \
                            </ul><br class=\"clear\" /></form>";
        //click remove
        $("a.btn-remove").live("click", function () {
            deleteReview($(this).attr("rel"));
            $("#pnlAddReview").show();
        });

        //click btn answer child review
        $(".btn-review a:not(a.btn-remove)").live("click", function (e) {
            $("form[id^='review_']").remove();
            $("#pnlAddReview").hide();

            var form = String.Format(sendBlock, $(this).attr("rel"), $(this).attr("rel").replace("review_", ""), localize("reviewsName"), localize("reviewsEmail"),
                                    localize("reviewsComent"), localize("reviewsSend"), localize("reviewsCancel"), localize("reviewsAnswer"));
            $(this).parent().after(form);

            initValidation($("form[id^='review_']"));

            //$(".btn-review a").show();

            $(this).hide();
            PIELoad("a.btn");

            e.preventDefault();
            e.cancelBubble = true;
            e.stopPropagation();
            return false;
        });

        //click btn answer
        $("#pnlAddReview a").live("click", function () {
            $('form[id^=review_]').remove();
            $("a", ".btn-review").show();
            if ($('form').valid()) {
                addReview(0, $("#txtName").val(), $("#txtEmail").val(), $("#txtMessage").val());
                $("#txtName").val("");
                $("#txtEmail").val("");
                $("#txtMessage").val("");
            }
        });

        //click button send child review
        $(".reviews-send a.btn-submit", "form[id^='review_']").live("click", function (e) {
            if ($("form[id^='review_']").valid()) {
                addReview($(this).attr("rel"), $("#txtName-reply").val(), $("#txtEmail-reply").val(), $("#txtMessage-reply").val());
                $("form[id^='review_']").remove();
                $("#pnlAddReview").show();
            }
        });

        //click button cancel child review
        $(".reviews-send a.btn-action", "form[id^='review_']").live("click", function (e) {
            PIEDeatch("#btn-cancel, #btn-send");
            $("form[id^='review_']").validate().resetForm();
            $("form[id^='review_']").remove();
            $("a", ".btn-review").show();
            $("#pnlAddReview").show();
        });

        function getReviews() {
            jQuery.ajax({
                url: 'httphandlers/reviews/getreviews.ashx',
                dataType: "JSON",
                cache: false,
                data: { entityId: options.entityId, entityType: options.entityType },
                success: function (data) {
                    var result = "";
                    var isAdmin = data[0];
                    for (var i = 1; i < data.length; i++) {
                        result += String.Format(itemHtml, data[i].name, data[i].date, data[i].text, "review_" + data[i].id, getChildren(data[i].children, isAdmin), isAdmin ? deleteRender(data[i].id) : "", localize("reviewsAnswer"));
                    }
                    $("#reviews").html(result);
                },
                error: function (data) {
                    notify(localize("reviewsError"), notifyType.error, true);
                }
            });
        }

        function addReview(parentId, name, email, text) {
            jQuery.ajax({
                url: 'httphandlers/reviews/addreview.ashx',
                dataType: "JSON",
                cache: false,
                data: {
                    entityId: options.entityId,
                    entityType: options.entityType,
                    parrentId: parentId,
                    name: htmlEncode(name),
                    email: htmlEncode(email),
                    text: htmlEncode(text)
                },
                type: "POST",
                success: function () {
                    PIEDeatch("#btn-cancel, #btn-send");
                    if ($("form[id^='review_']").length > 0) {
                        $("form[id^='review_']").validate().resetForm();
                    }
                    getReviews();
                },
                error: function (data) {
                    notify(localize("reviewsError"), notifyType.error, true);
                }
            });
        }

        function deleteReview(id) {
            $.ajax({
                url: "httphandlers/reviews/deletereview.ashx",
                data: { entityid: id },
                dataType: "text",
                cache: false,
                success: function () {
                    getReviews();
                },
                error: function () {
                    notify(localize("reviewsError"), notifyType.error, true);
                }
            });
        }

        function getChildren(children, isAdmin) {
            if (children == null) {
                return "";
            }
            var childrenText = "";
            for (var i = 0; i < children.length; i++) {
                childrenText += String.Format(itemHtml, children[i].name, children[i].date, children[i].text, "review_" + children[i].id, getChildren(children[i].children, isAdmin), isAdmin ? deleteRender(children[i].id) : "", localize("reviewsAnswer"));
            }
            return childrenText;
        }

        function deleteRender(id) {
            return String.Format("<a class=\"btn-remove\" href=\"javascript:void(0)\" rel=\"{0}\">{1}</a>", id, localize("reviewsDelete"));
        }

        return this.each(function () {
            getReviews();
        });

    };
})(jQuery);
