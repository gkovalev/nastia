(function ($) {

    var advantshop = window.Advantshop,
        scriptManager = advantshop.ScriptsManager,
        utilities = advantshop.Utilities, 
        $body = $(document.body);

    var reviews = function (selector, options) {

        if (options.entityId == null || options.entityType == null) {
            notify('Error in parameters reviews', notifyType.error, true);
        }

        this.$obj = advantshop.GetJQueryObject(selector);
        this.options = $.extend({}, this.defaultOptions, options);

        return this;
    };

    advantshop.NamespaceRequire('Advantshop.ScriptsManager');
    scriptManager.Reviews = reviews;

    reviews.prototype.InitTotal = function () {
        var objects = $('[data-plugin ="reviews"]');

        for (var i = 0, arrLength = objects.length; i < arrLength; i += 1) {
            reviews.prototype.Init(objects.eq(i), utilities.Eval(objects.eq(i).attr('data-reviews-options')) || {});
        }
    };

    $(reviews.prototype.InitTotal); // call document.ready

    reviews.prototype.Init = function (selector, options) {
        var reviewsObj = new reviews(selector, options);

        reviewsObj.GenerateHtml();

        reviewsObj.BindEvent();

        return reviewsObj;
    };

    reviews.prototype.GenerateHtml = function () {
        var reviewsObj = this;

        reviewsObj.GenerateReviews(function () {
            reviewsObj.GenerateForm(reviewsObj.$obj, 0);
        });
    };

    reviews.prototype.GenerateForm = function (container, parentId) {
        var reviewsObj = this;

        reviewsObj.Parts = reviewsObj.Parts || {};

        var $form = reviewsObj.Parts.$form;

        if ($form == null || $form.length === 0) {
            var html = new EJS({ url: 'js/plugins/reviews/templates/form.html' }).render();
            $form = $(html);
            reviewsObj.Parts.$form = $form;
        }

        var $btnCancel = $form.find('[data-reviews-action="cancel"]');

        if ($btnCancel.length > 0) {
            parentId === 0 ? $btnCancel.css('display', 'none') : $btnCancel.css('display', 'inline-block');
        }

        $form.attr('data-reviews-parentid', parentId);
        container.append($form);

        reviewsObj.ResetForm();

        return $form;
    };

    reviews.prototype.GenerateReviews = function (callback) {
        var reviewsObj = this;
        $.ajax({
            url: 'httphandlers/reviews/getreviews.ashx',
            dataType: 'JSON',
            type: 'POST',
            cache: false,
            data: { entityId: reviewsObj.options.entityId, entityType: reviewsObj.options.entityType },
            success: function (data) {
                var html = "";

                if (data != null) {
                    reviewsObj.$obj.find('[data-reviews-itemid]').remove();
                    html = new EJS({ url: 'js/plugins/reviews/templates/item.html' }).render(data);
                    reviewsObj.$obj.append(html);
                }

                if (callback != null) {
                    callback.call(reviewsObj, data, html);
                }
            },
            error: function (data) {
                notify(localize("reviewsError"), notifyType.error, true);
            }
        });
    };

    reviews.prototype.GetChild = function (children) { //call in template js/plugins/reviews/templates/item.html
        if (children == null) {
            return "";
        }

        var childrenObj = { Reviews: children };
        var html = new EJS({ url: 'js/plugins/reviews/templates/item.html' }).render(childrenObj);

        return html;
    };


    reviews.prototype.BindEvent = function () {
        var reviewsObj = this;

        if (utilities.Events.isExistEvent($body, 'click.reviewAdd') != true) {
            $body.on('click.reviewAdd', '[data-reviews-action="add"]', function () {

                var reviewForm = reviewsObj.Parts.$form;

                if (typeof reviewForm.valid === 'function') {
                    var isValid = reviewForm.valid();

                    if (isValid === false) {
                        return;
                    }
                }

                var arrParams = reviewForm.find('[data-reviews-param]');

                var params = {
                    entityId: reviewsObj.options.entityId,
                    entityType: reviewsObj.options.entityType,
                    parentId: reviewForm.attr('data-reviews-parentid'),
                    name: arrParams.filter('[data-reviews-param="name"]').val(),
                    email: arrParams.filter('[data-reviews-param="email"]').val(),
                    text: arrParams.filter('[data-reviews-param="text"]').val()
                };

                reviewsObj.Add(params);
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.reviewReply') != true) {
            $body.on('click.reviewReply', '[data-reviews-action="reply"]', function () {
                var review = $(this).closest('[data-reviews-itemid]');
                reviewsObj.GenerateForm($(this).parent(), review.attr('data-reviews-itemid'));
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.reviewDelete') != true) {
            $body.on('click.reviewDelete', '[data-reviews-action="delete"]', function () {
                var review = $(this).closest('[data-reviews-itemid]');
                var reviewId = review.attr('data-reviews-itemid');
                reviewsObj.Delete(reviewId);
            });
        }

        if (utilities.Events.isExistEvent($body, 'click.reviewCancel') != true) {
            $body.on('click.reviewCancel', '[data-reviews-action="cancel"]', function () {
                reviewsObj.GenerateForm(reviewsObj.$obj, 0);
            });
        }

    };

    reviews.prototype.Add = function (params) {
        var reviewsObj = this;

        $.ajax({
            url: 'httphandlers/reviews/addreview.ashx',
            dataType: 'JSON',
            type: 'POST',
            cache: false,
            data: params,
            success: function (data) {

                if (data === true) {
                    reviewsObj.GenerateHtml();
                } else {
                    notify(localize("reviewsError"), notifyType.error, true);
                }

            },
            error: function (data) {
                notify(localize("reviewsError"), notifyType.error, true);
            }
        });
    };

    reviews.prototype.Delete = function (id) {
        var reviewsObj = this;

        $.ajax({
            url: "httphandlers/reviews/deletereview.ashx",
            data: { entityid: id },
            type: 'POST',
            dataType: "text",
            cache: false,
            success: function () {
                reviewsObj.GenerateHtml();
            },
            error: function () {
                notify(localize("reviewsError"), notifyType.error, true);
            }
        });
    };

    reviews.prototype.ResetForm = function ($form) {
        var reviewsObj = this;
        $form = $form || reviewsObj.Parts.$form;
        $form[0].reset();
        initValidation($form);
    };

    reviews.prototype.defaultOptions = {
        entityId: null,
        entityType: null,
        maxlevel: 5
    };


})(jQuery);


